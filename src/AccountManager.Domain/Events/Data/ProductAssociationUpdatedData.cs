// <copyright file="ProductAssociationUpdatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents the payload for a product association updated event.
    /// </summary>
    public class ProductAssociationUpdatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product association.
        /// </summary>
        public Guid AssociationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account associated with the product.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        required public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the current status of the product association.
        /// </summary>
        required public string ProductStatus { get; set; }

        /// <summary>
        /// Gets or sets the action performed on the product association
        /// (e.g., Added, Updated, Removed).
        /// </summary>
        required public string Action { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the association was updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
