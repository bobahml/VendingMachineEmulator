using System;
using System.Collections.Generic;
using Xunit;

namespace VendingMachineBL.Tests
{
    public class CoinsExtensionTests
    {
        [Fact]
        public void AddTest1()
        {
            var coins1 = new List<Coin>
            {
                new Coin(10, 100),
                new Coin(5, 100),
                new Coin(2, 100),
                new Coin(1, 100),
            };


            var coins2 = new List<Coin>
            {
                new Coin(10, 1),
                new Coin(5, 1),
                new Coin(2, 1),
                new Coin(1, 1),
            };

            coins1.Add(coins2);


            Assert.True(CoinHelper.Expects(coins1, 10, 101));
            Assert.True(CoinHelper.Expects(coins1, 5, 101));
            Assert.True(CoinHelper.Expects(coins1, 2, 101));
            Assert.True(CoinHelper.Expects(coins1, 1, 101));
        }

        [Fact]
        public void AddTest2()
        {
            var coins1 = new List<Coin>
            {
                new Coin(10, 1),
                new Coin(1, 1),
            };


            var coins2 = new List<Coin>
            {
                new Coin(5, 1),
                new Coin(2, 1),
            };

            coins1.Add(coins2);


            Assert.True(CoinHelper.Expects(coins1, 10, 1));
            Assert.True(CoinHelper.Expects(coins1, 5, 1));
            Assert.True(CoinHelper.Expects(coins1, 2, 1));
            Assert.True(CoinHelper.Expects(coins1, 1, 1));
        }

        [Fact]
        public void SubtractTest1()
        {
            var coins1 = new List<Coin>
            {
                new Coin(10, 100),
                new Coin(5, 100),
                new Coin(2, 100),
                new Coin(1, 100),
            };


            var coins2 = new List<Coin>
            {
                new Coin(10, 1),
                new Coin(5, 1),
                new Coin(2, 1),
                new Coin(1, 1),
            };

            coins1.Subtract(coins2);


            Assert.True(CoinHelper.Expects(coins1, 10, 99));
            Assert.True(CoinHelper.Expects(coins1, 5, 99));
            Assert.True(CoinHelper.Expects(coins1, 2, 99));
            Assert.True(CoinHelper.Expects(coins1, 1, 99));
        }

        [Fact]
        public void SubtractTest2()
        {
            var coins1 = new List<Coin>
            {
                new Coin(5, 100),
                new Coin(2, 100),
                new Coin(1, 100),
            };


            var coins2 = new List<Coin>
            {
                new Coin(5, 100),
                new Coin(2, 100),
                new Coin(1, 100),
            };

            coins1.Subtract(coins2);


            Assert.True(CoinHelper.Expects(coins1, 5, 0));
            Assert.True(CoinHelper.Expects(coins1, 2, 0));
            Assert.True(CoinHelper.Expects(coins1, 1, 0));
        }

        [Fact]
        public void SubtractTest3()
        {
            var coins1 = new List<Coin>
            {
                new Coin(2, 100),
            };


            var coins2 = new List<Coin>
            {
                new Coin(2, 1),
                new Coin(1, 1),
            };


            Assert.Throws<InvalidOperationException>(() => coins1.Subtract(coins2));
        }

        [Fact]
        public void SubtractSelf()
        {
            var coins1 = new List<Coin>
            {
                new Coin(10, 100),
                new Coin(5, 100),
                new Coin(2, 100),
                new Coin(1, 100),
            };

            coins1.Subtract(coins1);


            Assert.True(CoinHelper.Expects(coins1, 10, 0));
            Assert.True(CoinHelper.Expects(coins1, 5, 0));
            Assert.True(CoinHelper.Expects(coins1, 2, 0));
            Assert.True(CoinHelper.Expects(coins1, 1, 0));
        }
    }
}
