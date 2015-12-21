using System.Collections.Generic;
using System.Linq;
using Moq;
using VendingMachine.Models;
using VendingMachine.Services.Interfaces;
using VendingMachine.ViewModels;
using VendingMachineBL;
using Xunit;
using Coin = VendingMachineBL.Coin;

namespace VendingMachine.Tests
{
    public class MainWindowViewModelTests
    {
        /*
         * 
           1. Система показывает кошелек пользователя (кол-во монет разного достоинства)

            1 руб = 10 штук (начальные данные)
            2 руб = 30 штук
            5 руб = 20 штук
            10 руб = 15 штук
         */
        [Fact]
        public void SystemShowsUserWalet()
        {
            var initializer = new Mock<ISystemInitializer>();
            var ititialValue = new[]
            {
                new Coin(1, 10),
                new Coin(2, 30),
                new Coin(5, 20),
                new Coin(10, 15)
            };
            initializer.Setup(s => s.GetUserCoins()).Returns(() => ititialValue);

            var vm = new MainWindowViewModel(initializer.Object, Mock.Of<IVendingMachineChangeCalculator>(), Mock.Of<ILocalizedMessageService>());
            vm.InitializeViewModelAsync().Wait();

            var results = vm.UserWallet.Coins;

            foreach (var item in ititialValue)
            {
                Assert.True(Expects(results, item.Denomination, item.Count));
            }
        }


        /*
        
            2. Система показывает ассортимент товаров для продажи, стоимость и остаток товара
             Чай = 13 руб, 10 порций. (начальные данные)
             Кофе = 18 руб, 20 порций.
             Кофе с молоком = 21 руб, 20 порций.
             Сок = 35 руб = 15 порций.
        */
        [Fact]
        public void SystemShowsGoodsForSale()
        {
            var initializer = new Mock<ISystemInitializer>();
            var ititialValue = new[]
            {
                new Product {Name = "Чай", Price = 13, Count = 10},
                new Product {Name = "Кофе", Price = 18, Count = 20},
                new Product {Name = "Кофе с молоком", Price = 21, Count = 20},
                new Product {Name = "Сок", Price = 35, Count = 15}
            };
            initializer.Setup(s => s.GetProducts()).Returns(() => ititialValue);

            var vm = new MainWindowViewModel(initializer.Object, Mock.Of<IVendingMachineChangeCalculator>(), Mock.Of<ILocalizedMessageService>());
            vm.InitializeViewModelAsync().Wait();

            var results = vm.Goods.Goods;

            foreach (var item in ititialValue)
            {
                Assert.True(Expects(results, item));
            }
        }


        /*
        
            3. Система показывает кошелек VM для сдачи (кол-во монет разного достоинства)
            1. 1 руб = 100 штук (начальные данные)
            2. 2 руб = 100 штук
            3. 5 руб = 100 штук
            4. 10 руб = 100 штук
        */
        [Fact]
        public void SystemShowsMachineWalet()
        {
            var initializer = new Mock<ISystemInitializer>();
            var ititialValue = new[]
            {
                new Coin(1, 100),
                new Coin(2, 100),
                new Coin(5, 100),
                new Coin(10, 100),
            };
            initializer.Setup(s => s.GetSystemCoins()).Returns(() => ititialValue);

            var vm = new MainWindowViewModel(initializer.Object, Mock.Of<IVendingMachineChangeCalculator>(), Mock.Of<ILocalizedMessageService>());
            vm.InitializeViewModelAsync().Wait();

            var results = vm.SystemWallet.Coins;

            foreach (var item in ititialValue)
            {
                Assert.True(Expects(results, item.Denomination, item.Count));
            }
        }


        /*
        4. Пользователь может ввнести монеты в монетоприемник VM нажав на монету (или кнопку "внести" рядом с соотвествующей монетой) в своем кошелке.
            1. При этом кол-во монет в кошелке пользователя соотвествущего достоинства должно измениться.
            2. VM должна обновить поле "Внесенная сумма".
        */
        [Fact]
        public void DropCoinToMachine()
        {
            #region arrange
            const int denomination = 5;
            var initialSystemCoins = new[] { new Coin(denomination, 100) };
            var ititialUserCoins = new[] { new Coin(denomination, 20) };

            var initializer = new Mock<ISystemInitializer>();
            initializer.Setup(s => s.GetSystemCoins()).Returns(() => initialSystemCoins);
            initializer.Setup(s => s.GetUserCoins()).Returns(() => ititialUserCoins);

            var vm = new MainWindowViewModel(initializer.Object, Mock.Of<IVendingMachineChangeCalculator>(), Mock.Of<ILocalizedMessageService>());
            vm.InitializeViewModelAsync().Wait();
            #endregion

            //act
            var c = vm.UserWallet.Coins.First(coin => coin.Denomination == denomination);
            vm.DropCoinCommand.Execute(c);



            //assert
            Assert.Equal(denomination, vm.UserBalance);
            initialSystemCoins.First(coin => coin.Denomination == denomination).Count++;
            foreach (var item in initialSystemCoins)
            {
                Assert.True(Expects(vm.SystemWallet.Coins, item.Denomination, item.Count));
            }

            ititialUserCoins.First(coin => coin.Denomination == denomination).Count--;
            foreach (var item in ititialUserCoins)
            {
                Assert.True(Expects(vm.UserWallet.Coins, item.Denomination, item.Count));
            }
        }


        /*
         5. Пользователь может запросить назад остаток внесенной суммы нажав кнопку "Сдача" на VM
            1. При этом кол-во монет в кошелке пользователя должно измениться.
            2. VM должна обновить поле "Внесенная сумма".
            3. Внесенная сумма возвращается целиком, при этом сумма возвращается наименьшим кол-вом монет. 
         * (напр: 23 руб = 2 х 10 руб + 1 х 2 руб + 1 х 1 руб). 
         * При этом возможно изменение кол-во монет в кошелке VM.
         
         */
        [Fact]
        public void GetChange()
        {
            #region arrange
            var initialSystemCoins = new[] 
            { 
                new Coin(1, 100),
                new Coin(2, 100),
                new Coin(5, 100),
                new Coin(10, 100),
            };
            var ititialUserCoins = new[] 
            {   
                new Coin(1, 23),
                new Coin(2, 0),
                new Coin(5, 0),
                new Coin(10, 0),
            };

            var initializer = new Mock<ISystemInitializer>();
            initializer.Setup(s => s.GetSystemCoins()).Returns(() => initialSystemCoins);
            initializer.Setup(s => s.GetUserCoins()).Returns(() => ititialUserCoins);

            var vm = new MainWindowViewModel(initializer.Object, new GreedyVendingMachineChangeCalculator(), Mock.Of<ILocalizedMessageService>());
            vm.InitializeViewModelAsync().Wait();
            #endregion

            //act
            var one = vm.UserWallet.Coins.First(coin => coin.Denomination == 1);
            for (int i = 0; i < 23; i++)
            {
                vm.DropCoinCommand.Execute(one);
            }

            vm.GetChangeCommand.Execute();


            //Assert
            var results = vm.UserWallet.Coins;
            var expected = new[]
            {
                new Coin(1, 1),
                new Coin(2, 1),
                new Coin(5, 0),
                new Coin(10, 2),
            };

            foreach (var item in expected)
            {
                Assert.True(Expects(results, item.Denomination, item.Count));
            }
        }


        /*
         6. Пользователь может купить товар нажав на товар (или на кнопку рядом с соотвествующим товаром) на VM
            1. Если стоимость товара <= "Внесенной суммы" товар выдается пользователю, "Внесенная сумма" уменьшается на цену товара 
         *      и сумма зачисляется в кошелек VM (см. п. 3). Пользователю показывается MessageBox с текстом "Спасибо!"
         */
        [Fact]
        public void Buy1()
        {
            #region arrange
            var initialSystemCoins = new[] { new Coin(10, 100) };
            var ititialUserCoins = new[] { new Coin(1, 23) };
            var products = new[] { new Product { Name = "Кофе", Price = 18, Count = 20 } };


            var initializer = new Mock<ISystemInitializer>();
            initializer.Setup(s => s.GetSystemCoins()).Returns(() => initialSystemCoins);
            initializer.Setup(s => s.GetUserCoins()).Returns(() => ititialUserCoins);
            initializer.Setup(s => s.GetProducts()).Returns(() => products);


            var messageService = new Mock<ILocalizedMessageService>();
            //Пользователю показывается MessageBox с текстом "Спасибо!
            messageService.Setup(s => s.ShowAsync("SuccessMessage")).Verifiable();

            var vm = new MainWindowViewModel(initializer.Object, new GreedyVendingMachineChangeCalculator(), messageService.Object);
            vm.InitializeViewModelAsync().Wait();
            #endregion

            #region act
            var systemBalance = initialSystemCoins.Sum();
            var one = vm.UserWallet.Coins.First(coin => coin.Denomination == 1);
            for (var i = 0; i < 23; i++)
            {
                vm.DropCoinCommand.Execute(one);
            }

            var ballance = vm.UserBalance;
            var product = vm.Goods.Goods.First(g => g.Name == "Кофе");
            var productPrice = product.Price;
            var productCount = product.Count;

            vm.BuyCommand.Execute(product);

            #endregion



            //Assert
            Assert.Equal(ballance - productPrice, vm.UserBalance);
            Assert.Equal(systemBalance + productPrice, vm.GetSystemBalance());
            Assert.Equal(productCount - 1, vm.Goods.Goods.First(g => g.Name == "Кофе").Count);
            messageService.Verify();
        }

        /*
       6. Пользователь может купить товар нажав на товар (или на кнопку рядом с соотвествующим товаром) на VM
          2. Если стоимость товара > "Внесенной суммы" пользователю выдается MessageBox с текстом "Недостаточно средств"
       */
        [Fact]
        public void Buy2()
        {
            #region arrange
            var initializer = new Mock<ISystemInitializer>();
            initializer.Setup(s => s.GetProducts()).Returns(() => new[] { new Product { Name = "Кофе", Price = 18, Count = 20 } });
            initializer.Setup(s => s.GetUserCoins()).Returns(() => new[] { new Coin(10, 1) });
            initializer.Setup(s => s.GetSystemCoins()).Returns(() => new Coin[0]);

            var messageService = new Mock<ILocalizedMessageService>();
            //Пользователю показывается MessageBox с текстом "Недостаточно средств"
            messageService.Setup(s => s.ShowWarningAsync("NotEnoughMoneyMessage")).Verifiable();

            var vm = new MainWindowViewModel(initializer.Object, new GreedyVendingMachineChangeCalculator(), messageService.Object);
            vm.InitializeViewModelAsync().Wait();
            #endregion

            #region act
            var one = vm.UserWallet.Coins.First(coin => coin.Denomination == 10);
            vm.DropCoinCommand.Execute(one);

            var ballance = vm.UserBalance;
            var product = vm.Goods.Goods.First(g => g.Name == "Кофе");
            var productCount = product.Count;

            vm.BuyCommand.Execute(product);

            #endregion



            //Assert
            Assert.Equal(ballance, vm.UserBalance);
            Assert.Equal(0, vm.GetSystemBalance());
            Assert.Equal(productCount, vm.Goods.Goods.First(g => g.Name == "Кофе").Count);
            messageService.Verify();
        }



        /*
         * 7. Пользователь может повторить п.4. п.5. п.6. в произвольной последовательности.
            */

        [Fact]
        public void ComplexTest1()
        {
            #region arrange
            var initialSystemCoins = new[] 
            { 
                new Coin(1, 100),
                new Coin(2, 100),
                new Coin(5, 100),
                new Coin(10, 100),
            };
            var ititialUserCoins = new[] 
            {   
                new Coin(1, 10),
                new Coin(2, 30),
                new Coin(5, 20),
                new Coin(10, 15)
            };
            var products = new[] { new Product { Name = "Кофе", Price = 18, Count = 20 } };


            var initializer = new Mock<ISystemInitializer>();
            initializer.Setup(s => s.GetSystemCoins()).Returns(() => initialSystemCoins);
            initializer.Setup(s => s.GetUserCoins()).Returns(() => ititialUserCoins);
            initializer.Setup(s => s.GetProducts()).Returns(() => products);

            var vm = new MainWindowViewModel(initializer.Object, new GreedyVendingMachineChangeCalculator(), Mock.Of<ILocalizedMessageService>());
            vm.InitializeViewModelAsync().Wait();
            #endregion

            #region act
            //бросить в автомат 20 р
            vm.DropCoinCommand.Execute(vm.UserWallet.Coins.First(coin => coin.Denomination == 10));
            vm.DropCoinCommand.Execute(vm.UserWallet.Coins.First(coin => coin.Denomination == 10));

            //купить кофе
            var product = vm.Goods.Goods.First(g => g.Name == "Кофе");
            vm.BuyCommand.Execute(product);

            //забрать сдачу
            vm.GetChangeCommand.Execute();
            #endregion



            //Assert
            var expectedUserCoins = new[]
            {
                new Coin(1, 10),
                new Coin(2, 31),
                new Coin(5, 20),
                new Coin(10, 13)
            };
            foreach (var item in expectedUserCoins)
            {
                Assert.True(Expects(vm.UserWallet.Coins, item.Denomination, item.Count));
            }


            var expectedSystemCoins = new[]
            {
                new Coin(1, 100),
                new Coin(2, 99),
                new Coin(5, 100),
                new Coin(10, 102),
            };
            foreach (var item in expectedSystemCoins)
            {
                Assert.True(Expects(vm.SystemWallet.Coins, item.Denomination, item.Count));
            }

            Assert.Equal(19, vm.Goods.Goods.First(g => g.Name == "Кофе").Count);

        }


        static bool Expects(IEnumerable<Models.Coin> items, int denomination, int count)
        {
            var c = items.FirstOrDefault(x => x.Denomination == denomination);
            return c != null && c.Count == count;
        }

        static bool Expects(IEnumerable<Product> items, Product p)
        {
            return items.Any(c => c.Name == p.Name && c.Count == p.Count && c.Price == p.Price);
        }
    }
}
