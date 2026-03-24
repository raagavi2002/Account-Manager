// <copyright file="ProductAssociationDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Represents a product associated with an account.
    /// </summary>
    public class ProductAssociationDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the product.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the product association is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the UTC time when the product was last synchronized.
        /// </summary>
        public DateTime LastSyncedAt { get; set; }
    }
}
