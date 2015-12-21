using System.Collections.Generic;
using VendingMachine.Models;
using VendingMachine.Services.Interfaces;
using Coin = VendingMachineBL.Coin;

namespace VendingMachine.Services
{
    internal class SystemInitializer : ISystemInitializer
    {
        public ICollection<Coin> GetUserCoins()
        {

            /*
            (начальные данные)
             1 руб = 10 штук 
             2 руб = 30 штук
             5 руб = 20 штук
             10 руб = 15 штук
            */
            return new[]
            {
                new Coin(1, 10),
                new Coin(2, 30),
                new Coin(5, 20),
                new Coin(10, 15),
            };
        }

        public ICollection<Coin> GetSystemCoins()
        {
            /*
                (начальные данные)
                1 руб = 100 штук
                2 руб = 100 штук
                5 руб = 100 штук
                10 руб = 100 штук
                */
            return new[]
            {
                new Coin(1, 100),
                new Coin(2, 100),
                new Coin(5, 100),
                new Coin(10, 100),
            };
        }

        public ICollection<Product> GetProducts()
        {
            /*
            (начальные данные)
            Чай = 13 руб, 10 порций. 
            Кофе = 18 руб, 20 порций.
            Кофе с молоком = 21 руб, 20 порций.
            Сок = 35 руб = 15 порций.
            */
            return new[]
            {
                new Product {Name = "Чай", Price = 13, Count = 10},
                new Product {Name = "Кофе", Price = 18, Count = 20},
                new Product {Name = "Кофе с молоком", Price = 21, Count = 20},
                new Product {Name = "Сок", Price = 35, Count = 15}
            };
        }
    }
}