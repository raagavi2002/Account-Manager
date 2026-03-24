// <copyright file="ProductAssociatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.External
{
    /// <summary>
    /// Represents the payload for a product associated event.
    /// </summary>
    public class ProductAssociatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated account.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        required public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the effective date of the association.
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the association occurred.
        /// </summary>
        public DateTime AssociatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that created the association.
        /// </summary>
        required public string AssociatedBy { get; set; }
    }
}
