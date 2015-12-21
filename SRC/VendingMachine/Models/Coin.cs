using Catel.Data;

namespace VendingMachine.Models
{
    public class Coin : ModelBase
    {
        public Coin(int denomination, int count)
        {
            Denomination = denomination;
            Count = count;
        }

        #region Properties

        #region Denomination property

        /// <summary>
        /// Gets or sets the Nominal value.
        /// </summary>
        public int Denomination
        {
            get { return GetValue<int>(DenominationProperty); }
            private set { SetValue(DenominationProperty, value); }
        }

        /// <summary>
        /// Nominal property data.
        /// </summary>
        public static readonly PropertyData DenominationProperty = RegisterProperty("Denomination", typeof (int));

        #endregion
        
        #region Count property

        /// <summary>
        /// Gets or sets the Count value.
        /// </summary>
        public int Count
        {
            get { return GetValue<int>(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        /// <summary>
        /// Count property data.
        /// </summary>
        public static readonly PropertyData CountProperty = RegisterProperty("Count", typeof(int));

        #endregion

        #endregion

    }
}
