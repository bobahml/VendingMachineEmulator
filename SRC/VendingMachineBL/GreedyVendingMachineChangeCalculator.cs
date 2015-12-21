using System;
using System.Collections.Generic;
using System.Linq;


namespace VendingMachineBL
{
    public class GreedyVendingMachineChangeCalculator : IVendingMachineChangeCalculator
    {
        public IList<Coin> Calculate(ICollection<Coin> coins, int change)
        {
            if (coins == null) throw new ArgumentNullException("coins");
            if (coins.Count == 0) throw new ArgumentException("Argument is empty collection", "coins");
            if (change <= 0) throw new ArgumentOutOfRangeException("change");


            var orderedCoins = coins.OrderByDescending(c => c.Denomination);
            return CalculateInternal(orderedCoins, change);
        }



        private static IList<Coin> CalculateInternal(IEnumerable<Coin> coins, int change)
        {
            var result = new List<Coin>();
            var changeLeft = change;

            foreach (var coin in coins)
            {
                if (changeLeft <= 0)
                {
                    break;
                }

                if (coin.Count <= 0 || coin.Denomination > changeLeft)
                    continue;


                var howMany = Math.Min(coin.Count, changeLeft / coin.Denomination);
                result.Add(new Coin(coin.Denomination, howMany));

                var amount = howMany * coin.Denomination;
                changeLeft = changeLeft - amount;
            }

            return result;
        }
    }
}
