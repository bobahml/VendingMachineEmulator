using System.Collections.Generic;
using System.Linq;

namespace VendingMachineBL.Tests
{
    internal static class CoinHelper
    {
        public static bool Expects(IList<Coin> coins, int denomination, int count)
        {
            var c = coins.FirstOrDefault(x => x.Denomination == denomination);
            return c == null ? false : c.Count == count;
        }
    }
}