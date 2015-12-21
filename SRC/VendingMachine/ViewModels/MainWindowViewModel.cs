using Catel.Data;
using VendingMachine.Models;
using VendingMachine.Services.Interfaces;
using VendingMachineBL;
using Coin = VendingMachine.Models.Coin;
using Catel.MVVM;
using System.Threading.Tasks;

namespace VendingMachine.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ISystemInitializer _systemInitializer;
        private readonly IVendingMachineChangeCalculator _vendingMachineChangeCalculator;
        private readonly ILocalizedMessageService _messageService;

        public MainWindowViewModel(ISystemInitializer systemInitializer,
            IVendingMachineChangeCalculator vendingMachineChangeCalculator,
            ILocalizedMessageService messageService
            )
        {
            _systemInitializer = systemInitializer;
            _vendingMachineChangeCalculator = vendingMachineChangeCalculator;
            _messageService = messageService;

            GoodsStorage = new GoodsStorageViewModel(_systemInitializer.GetProducts());
        }

        public override string Title { get { return "Vending Machine Emulator"; } }

        #region UserWallet property

        /// <summary>
        /// Gets or sets the UserWallet value.
        /// </summary>
        public WalletViewModel UserWallet
        {
            get { return GetValue<WalletViewModel>(UserWalletProperty); }
            private set { SetValue(UserWalletProperty, value); }
        }

        /// <summary>
        /// UserWallet property data.
        /// </summary>
        public static readonly PropertyData UserWalletProperty = RegisterProperty("UserWallet", typeof(WalletViewModel),
            () => new WalletViewModel { CanUserChange = true });

        #endregion

        #region UserBalance property

        /// <summary>
        /// Gets or sets the UserBalance value.
        /// </summary>
        public int UserBalance
        {
            get { return GetValue<int>(UserBalanceProperty); }
            private set { SetValue(UserBalanceProperty, value); }
        }

        /// <summary>
        /// UserBalance property data.
        /// </summary>
        public static readonly PropertyData UserBalanceProperty = RegisterProperty("UserBalance", typeof (int));

        #endregion

        #region SystemWallet property

        /// <summary>
        /// Gets or sets the SystemWallet value.
        /// </summary>
        public WalletViewModel SystemWallet
        {
            get { return GetValue<WalletViewModel>(SystemWalletProperty); }
            private set { SetValue(SystemWalletProperty, value); }
        }

        /// <summary>
        /// SystemWallet property data.
        /// </summary>
        public static readonly PropertyData SystemWalletProperty = RegisterProperty("SystemWallet", typeof(WalletViewModel),
                  () => new WalletViewModel());

        #endregion

        #region GoodsStorage property

        /// <summary>
        /// Gets or sets the GoodsStorage value.
        /// </summary>
        public GoodsStorageViewModel GoodsStorage
        {
            get { return GetValue<GoodsStorageViewModel>(GoodsStorageProperty); }
            set { SetValue(GoodsStorageProperty, value); }
        }

        /// <summary>
        /// GoodsStorage property data.
        /// </summary>
        public static readonly PropertyData GoodsStorageProperty = RegisterProperty("GoodsStorage", typeof(GoodsStorageViewModel));

        #endregion

        #region TransactionExecuting property

        /// <summary>
        /// Gets or sets the TransactionExecuting value.
        /// </summary>
        public bool TransactionExecuting
        {
            get { return GetValue<bool>(TransactionExecutingProperty); }
            private set { SetValue(TransactionExecutingProperty, value); }
        }

        /// <summary>
        /// TransactionExecuting property data.
        /// </summary>
        public static readonly PropertyData TransactionExecutingProperty = RegisterProperty("TransactionExecuting", typeof(bool));

        #endregion

        #region Commands

        #region GetChange command

        private Command _getChangeCommand;

        /// <summary>
        /// Gets the GetChange command.
        /// </summary>
        public Command GetChangeCommand
        {
            get { return _getChangeCommand ?? (_getChangeCommand = new Command(GetChange, CanGetChange)); }
        }

        /// <summary>
        /// Method to invoke when the GetChange command is executed.
        /// </summary>
        private void GetChange()
        {
            TransactionExecuting = true;
            try
            {
                var systemCoins = SystemWallet.CopyCoins();
                var usercoins = UserWallet.CopyCoins();

                var change = _vendingMachineChangeCalculator.Calculate(systemCoins, UserBalance);
                UserBalance -= change.Sum();
                systemCoins.Subtract(change);
                usercoins.Add(change);
                SystemWallet.SetCoins(systemCoins);
                UserWallet.SetCoins(usercoins);
            }
            finally
            {
                TransactionExecuting = false;
            }         
        }

        /// <summary>
        /// Method to check whether the GetChange command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanGetChange()
        {
            return !TransactionExecuting && UserBalance > 0;
        }

        #endregion
        
        #region Buy command

        private TaskCommand<Product> _buyCommand;

        /// <summary>
        /// Gets the Buy command.
        /// </summary>
        public TaskCommand<Product> BuyCommand
        {
            get { return _buyCommand ?? (_buyCommand = new TaskCommand<Product>(Buy, p => !TransactionExecuting)); }
        }

        /// <summary>
        /// Method to invoke when the Buy command is executed.
        /// </summary>
        private async Task Buy(Product p)
        {
            TransactionExecuting = true;
            try
            {
                if (p.Count < 1)
                    return;

                if (UserBalance < p.Price)
                {
                    await _messageService.ShowAsync("NotEnoughMoneyMessage");
                    return;
                }

                UserBalance -= p.Price;
                p.Count--;

                await _messageService.ShowAsync("SuccessMessage");
            }
            finally
            {
                TransactionExecuting = false;
            }
        }

        #endregion

        #region DropCoin command

        private Command<Coin> _dropCoinCommand;

        /// <summary>
        /// Gets the DropCoin command.
        /// </summary>
        public Command<Coin> DropCoinCommand
        {
            get { return _dropCoinCommand ?? (_dropCoinCommand = new Command<Coin>(DropCoinToVm, c=> !TransactionExecuting)); }
        }

        /// <summary>
        /// Method to invoke when the DropCoin command is executed.
        /// </summary>
        private void DropCoinToVm(Coin c)
        {
            TransactionExecuting = true;
            try
            {
                c.Count --;

                UserBalance += c.Denomination;
                SystemWallet.IncCoin(c.Denomination);
            }
            finally
            {
                TransactionExecuting = false;
            }
        }

        #endregion
        
        #endregion


        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var userCoins = _systemInitializer.GetUserCoins();
            UserWallet.SetCoins(userCoins);

            var systemCoins = _systemInitializer.GetSystemCoins();
            SystemWallet.SetCoins(systemCoins);
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
