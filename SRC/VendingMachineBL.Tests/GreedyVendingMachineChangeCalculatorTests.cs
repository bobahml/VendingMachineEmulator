using System.Collections.Generic;
using Xunit;

namespace VendingMachineBL.Tests
{
    public class GreedyVendingMachineChangeCalculatorTests
    {
        [Fact]
        public void Test1()
        {
            var coins = new List<Coin>
            {
                new Coin(10, 100),
                new Coin(5, 100),
                new Coin(2, 100),
                new Coin(1, 100),
            };


            var calc = new GreedyVendingMachineChangeCalculator();
            var results = calc.Calculate(coins, 15);
            Assert.Equal(2, results.Count);
            Assert.True(CoinHelper.Expects(results, 10, 1));
            Assert.True(CoinHelper.Expects(results, 5, 1));
        }

        [Fact]
        public void Test2()
        {
            var coins = new List<Coin>
            {
                new Coin(10, 100),
                new Coin(5, 100),
                new Coin(2, 100),
                new Coin(1, 100),
            };
            var calc = new GreedyVendingMachineChangeCalculator();
            var results = calc.Calculate(coins, 1);
            Assert.Equal(1, results.Count);
            Assert.True(CoinHelper.Expects(results, 1, 1));
        }

        [Fact]
        public void Test3()
        {
            var coins = new List<Coin>
            {
                new Coin(10, 1),
                new Coin(5, 1),
                new Coin(2, 100),
                new Coin(1, 100),
            };
            var calc = new GreedyVendingMachineChangeCalculator();
            var results = calc.Calculate(coins, 20);
            Assert.Equal(4, results.Count);
            Assert.True(CoinHelper.Expects(results, 10, 1));
            Assert.True(CoinHelper.Expects(results, 5, 1));
            Assert.True(CoinHelper.Expects(results, 2, 2));
            Assert.True(CoinHelper.Expects(results, 1, 1));
        }

        [Fact]
        public void NoMatchCoins()
        {
            var coins = new List<Coin>
              {
                 new Coin(10, 0),
                 new Coin(5, 0),
                 new Coin(2, 0),
                 new Coin(1, 0),
              };
            var calc = new GreedyVendingMachineChangeCalculator();
            Assert.Empty(calc.Calculate(coins, 20));
        }

        [Fact]
        public void NoMatchDueToNotEnoughCoins()
        {
            var coins = new List<Coin>
            {
                new Coin(10, 5),
                 new Coin(5, 0),
                 new Coin(2, 0),
                 new Coin(1, 0),
              };
            var calc = new GreedyVendingMachineChangeCalculator();
            var res = calc.Calculate(coins, 100);

            Assert.True(CoinHelper.Expects(res, 10, 5));
        }

        [Fact]
        public void Test4()
        {
            var coins = new List<Coin>
              {
                 new Coin(10, 1),
                 new Coin(5, 1),
                 new Coin(2, 100),
                 new Coin(1, 100),
              };
            var calc = new GreedyVendingMachineChangeCalculator();
            var results = calc.Calculate(coins, 3);
            Assert.Equal(2, results.Count);
            Assert.True(CoinHelper.Expects(results, 2, 1));
            Assert.True(CoinHelper.Expects(results, 1, 1));
        }

        [Fact]
        public void Test5()
        {
            var coins = new List<Coin>
              {
                 new Coin(10, 0),
                 new Coin(5, 0),
                 new Coin(2, 0),
                 new Coin(1, 100),
              };
            var calc = new GreedyVendingMachineChangeCalculator();
            var results = calc.Calculate(coins, 34);
            Assert.Equal(1, results.Count);
            Assert.True(CoinHelper.Expects(results, 1, 34));
        }

        [Fact]
        public void Test6()
        {
            var coins = new List<Coin>
              {
                 new Coin(50, 2),
                 new Coin(20, 1),
                 new Coin(10, 4),
                 new Coin(1, int.MaxValue),
              };
            var calc = new GreedyVendingMachineChangeCalculator();
            var results = calc.Calculate(coins, 98);
            Assert.Equal(4, results.Count);
            Assert.True(CoinHelper.Expects(results, 50, 1));
            Assert.True(CoinHelper.Expects(results, 20, 1));
            Assert.True(CoinHelper.Expects(results, 10, 2));
            Assert.True(CoinHelper.Expects(results, 1, 8));
        }

        [Fact]
        public void Test7()
        {
            var coins = new List<Coin>
              {
                 new Coin(50, 1),
                 new Coin(20, 2),
                 new Coin(15, 1),
                 new Coin(10, 1),
                 new Coin(1, 8),
              };
            var calc = new GreedyVendingMachineChangeCalculator();
            var results = calc.Calculate(coins, 98);
            Assert.Equal(3, results.Count);
            Assert.True(CoinHelper.Expects(results, 50, 1));
            Assert.True(CoinHelper.Expects(results, 20, 2));
            Assert.True(CoinHelper.Expects(results, 1, 8));
        }


        [Fact]
        public void Test8()
        {

            var coins = new List<Coin>
              {
                 new Coin(10, 10),
                 new Coin(5, 10),
                 new Coin(2, 10),
                 new Coin(1, 10)
              };
            var calc = new GreedyVendingMachineChangeCalculator();
            var results = calc.Calculate(coins, 23);
            Assert.Equal(3, results.Count);
            Assert.True(CoinHelper.Expects(results, 10, 2));
            Assert.True(CoinHelper.Expects(results, 2, 1));
            Assert.True(CoinHelper.Expects(results, 1, 1));
        }
    }
}
