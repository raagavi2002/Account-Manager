// <copyright file="AccountProductDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// DTO representing a product associated with an account.
    /// Used for mapping product-account relationships in workflows,
    /// ensuring auditability and compliance.
    /// </summary>
    public class AccountProductDto
    {
        /// <summary>
        /// Gets or sets Unique identifier of the product.
        /// Links to the product catalog or repository entry.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets Human-readable name of the product.
        /// Useful for UI display and reporting.
        /// </summary>
        public string ProductName { get; set; } = default!;

        /// <summary>
        /// Gets or sets Current status of the product (e.g., Active, Inactive, Pending).
        /// Enables conditional validation and business rule enforcement.
        /// </summary>
        public string ProductStatus { get; set; } = default!;

        /// <summary>
        /// Gets or sets Indicates whether the product is currently active for the account.
        /// Boolean flag used in filtering and compliance checks.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets Date when the product association expires, if applicable.
        /// Null if the product has no expiration.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets Timestamp when the product association was created.
        /// Used for audit trails and historical reporting.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets Timestamp of the last synchronization with external systems.
        /// Null if the product has not yet been synced.
        /// </summary>
        public DateTime? LastSyncedAt { get; set; }
    }
}
