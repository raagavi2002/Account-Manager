// <copyright file="ProductUpdatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.External
{
    /// <summary>
    /// Represents the payload for a product updated event.
    /// </summary>
    public class ProductUpdatedData
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
        /// Gets or sets the collection of changed fields, keyed by field name.
        /// </summary>
        required public Dictionary<string, ProductFieldChange> ChangedFields { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the product was updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
