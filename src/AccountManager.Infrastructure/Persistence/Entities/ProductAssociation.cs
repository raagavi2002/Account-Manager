// <copyright file="ProductAssociation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the association between an account and a product.
/// Tracks product subscriptions, licenses, or entitlements for accounts.
/// </summary>
[Table("product_associations", Schema = "am")]
[Index("AccountId", Name = "idx_product_account")]
[Index("AccountId", "ProductId", Name = "uq_product_account", IsUnique = true)]
public partial class ProductAssociation
{
    /// <summary>
    /// Gets or sets the unique identifier for the product association.
    /// </summary>
    [Key]
    [Column("product_association_id")]
    public int ProductAssociationId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the account associated with the product.
    /// </summary>
    [Column("account_id")]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the product associated with the account.
    /// </summary>
    [Column("product_id")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    [Column("product_name")]
    [StringLength(255)]
    public string ProductName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the current status of the product association (e.g., ACTIVE, EXPIRED, SUSPENDED).
    /// </summary>
    [Column("product_status")]
    [StringLength(50)]
    public string ProductStatus { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the product association is currently active.
    /// </summary>
    [Column("is_active")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the expiration date for the product association, if applicable (optional).
    /// </summary>
    [Column("expiration_date")]
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the product association was created.
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the product association was last updated.
    /// </summary>
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the product association was last synchronized with external systems.
    /// </summary>
    [Column("last_synced_at")]
    public DateTime LastSyncedAt { get; set; }

    /// <summary>
    /// Gets or sets the version number for optimistic concurrency control.
    /// </summary>
    [Column("version")]
    public int Version { get; set; }
}
