using System.Collections.Generic;

namespace VendingMachineBL
{
    public interface IVendingMachineChangeCalculator
    {
        IList<Coin> Calculate(ICollection<Coin> coins, int change);
    }
}