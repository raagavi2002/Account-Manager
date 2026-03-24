// <copyright file="ProductPricing.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.External
{
    /// <summary>
    /// Represents pricing information for a product.
    /// </summary>
    public class ProductPricing
    {
        /// <summary>
        /// Gets or sets the base price of the product.
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// Gets or sets the currency code for the price.
        /// </summary>
        required public string Currency { get; set; }
    }
}
