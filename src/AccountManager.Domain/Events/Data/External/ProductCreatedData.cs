// <copyright file="ProductCreatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.External
{
    /// <summary>
    /// Represents the payload for a product created event.
    /// </summary>
    public class ProductCreatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        required public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account the product belongs to.
        /// </summary>
        required public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        required public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the current status of the product.
        /// </summary>
        required public string ProductStatus { get; set; }

        /// <summary>
        /// Gets or sets the pricing details for the product.
        /// </summary>
        required public ProductPricing Pricing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is active.
        /// </summary>
        required public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the date when the product becomes effective.
        /// </summary>
        required public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the product expires, if applicable.
        /// </summary>
        required public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the product was created.
        /// </summary>
        required public DateTime CreatedAt { get; set; }
    }
}
