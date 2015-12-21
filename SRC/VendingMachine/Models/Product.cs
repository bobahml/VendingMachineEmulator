using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Catel.Data;

namespace VendingMachine.Models
{
    /// <summary>
    /// $Name$ model which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class Product : ModelBase
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new object from scratch.
        /// </summary>
        public Product()
        {
        }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new object based on <see cref="SerializationInfo"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains the information.</param>
        /// <param name="context"><see cref="StreamingContext"/>.</param>
        protected Product(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        #endregion

        #region Properties

        #region Name property

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Name property data.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof (string));

        #endregion

        #region Price property

        /// <summary>
        /// Gets or sets the Price value.
        /// </summary>
        public int Price
        {
            get { return GetValue<int>(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        /// <summary>
        /// Price property data.
        /// </summary>
        public static readonly PropertyData PriceProperty = RegisterProperty("Price", typeof (int));

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

        #region Methods

        /// <summary>
        /// Validates the business rules of this object. Override this method to enable
        ///             validation of business rules.
        /// </summary>
        /// <param name="validationResults">The validation results, add additional results to this list.</param>
        protected override void ValidateBusinessRules(List<IBusinessRuleValidationResult> validationResults)
        {
            base.ValidateBusinessRules(validationResults);
        }

        /// <summary>
        /// Validates the field values of this object. Override this method to enable
        ///             validation of field values.
        /// </summary>
        /// <param name="validationResults">The validation results, add additional results to this list.</param>
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            base.ValidateFields(validationResults);
        }

        #endregion
    }
}
