using System.Collections.Generic;
using System.Collections.ObjectModel;
using Catel.Data;
using VendingMachine.Models;
using Catel.MVVM;

namespace VendingMachine.ViewModels
{
    public class GoodsStorageViewModel : ViewModelBase
    {
        public GoodsStorageViewModel(IEnumerable<Product> products)
        {
            Goods = new ObservableCollection<Product>(products);
        }


        public override string Title { get { return "View model title"; } }
        #region Goods property

        /// <summary>
        /// Gets or sets the Goods value.
        /// </summary>
        public ObservableCollection<Product> Goods
        {
            get { return GetValue<ObservableCollection<Product>>(GoodsProperty); }
            private set { SetValue(GoodsProperty, value); }
        }

        /// <summary>
        /// Goods property data.
        /// </summary>
        public static readonly PropertyData GoodsProperty = RegisterProperty("Goods", typeof (ObservableCollection<Product>), 
            ()=>new ObservableCollection<Product>());

        #endregion

    }
}
