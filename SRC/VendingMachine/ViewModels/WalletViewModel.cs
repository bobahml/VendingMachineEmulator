using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Catel.Data;
using VendingMachine.Models;

namespace VendingMachine.ViewModels
{
    using Catel.MVVM;

    public class WalletViewModel : ViewModelBase
    {

        #region Coins property

        /// <summary>
        /// Gets or sets the Coins value.
        /// </summary>
        public ObservableCollection<Coin> Coins
        {
            get { return GetValue<ObservableCollection<Coin>>(CoinsProperty); }
            private set { SetValue(CoinsProperty, value); }
        }

        /// <summary>
        /// Coins property data.
        /// </summary>
        public static readonly PropertyData CoinsProperty = RegisterProperty("Coins", typeof(ObservableCollection<Coin>), ()=>new ObservableCollection<Coin>());

        #endregion






        public IList<VendingMachineBL.Coin> CopyCoins()
        {
            return Coins.Select(coin => new VendingMachineBL.Coin(coin.Denomination, coin.Count)).ToList();
        }

        public void SetCoins(ICollection<VendingMachineBL.Coin> coins)
        {
            Coins = new ObservableCollection<Coin>(coins.Select(c => new Coin(c.Denomination, c.Count)));
        }


        public void IncCoin(int denomination)
        {
            var coin = Coins.FirstOrDefault(c=>c.Denomination == denomination);
            if (coin == null)
            {
                coin = new Coin(denomination, 1);
                Coins.Add(coin);
            }
            else
            {
                coin.Count ++;
            }

        }
    }
}
