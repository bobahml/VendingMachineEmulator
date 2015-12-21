using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineBL
{
    public static class CoinsExtension
    {
        public static int Sum(this IList<Coin> coins)
        {
            if (coins == null) throw new ArgumentNullException("coins");

            return coins.Sum(c => c.Count*c.Denomination);
        }



        public static void Add(this IList<Coin> coins1, IList<Coin> coins2)
        {
            if (coins1 == null) throw new ArgumentNullException("coins1");
            if (coins2 == null) throw new ArgumentNullException("coins2");


            foreach (var coin in coins2)
            {
                var s = coins1.FirstOrDefault(c => c.Denomination == coin.Denomination);
                if (s == null)
                {
                    coins1.Add(coin);
                }
                else
                {
                    s.Count += coin.Count;
                }
            }
        }

        public static void Subtract(this IList<Coin> coins1, IList<Coin> coins2)
        {
            if (coins1 == null) throw new ArgumentNullException("coins1");
            if (coins2 == null) throw new ArgumentNullException("coins2");

            foreach (var coin in coins2)
            {
                var s = coins1.FirstOrDefault(c => c.Denomination == coin.Denomination);
                if (s == null || s.Count < coin.Count)
                {
                    throw  new InvalidOperationException("subtraction logic broken");
                }
                else
                {
                    s.Count -= coin.Count;
                }
            }
        }
    }
}