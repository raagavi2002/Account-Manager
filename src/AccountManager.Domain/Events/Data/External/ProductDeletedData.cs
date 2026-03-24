// <copyright file="ProductDeletedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.External
{
    /// <summary>
    /// Represents the payload for a product deleted event.
    /// </summary>
    public class ProductDeletedData
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
        /// Gets or sets the UTC timestamp when the product was deleted.
        /// </summary>
        public DateTime DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the reason for deleting the product.
        /// </summary>
        required public string Reason { get; set; }
    }
}
