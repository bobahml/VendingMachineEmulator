using System.Collections.Generic;
using VendingMachine.Models;
using Coin = VendingMachineBL.Coin;

namespace VendingMachine.Services.Interfaces
{
    public interface ISystemInitializer
    {
        ICollection<Coin> GetUserCoins();
        ICollection<Coin> GetSystemCoins();
        ICollection<Product> GetProducts();

    }
}