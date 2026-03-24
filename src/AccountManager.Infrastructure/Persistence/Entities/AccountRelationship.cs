// <copyright file="AccountRelationship.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a hierarchical relationship between a head account and a sub-account.
/// </summary>
[Table("account_relationships", Schema = "am")]
public partial class AccountRelationship
{
    /// <summary>
    /// Gets or sets the unique identifier for the account relationship.
    /// </summary>
    [Key]
    [Column("account_relationship_id")]
    public int AccountRelationshipId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who established this relationship.
    /// </summary>
    [Column("established_by")]
    public int EstablishedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the relationship was established.
    /// </summary>
    [Column("established_at")]
    public DateTime EstablishedAt { get; set; }

    /// <summary>
    /// Gets or sets the current status of the relationship (e.g., ACTIVE, INACTIVE).
    /// </summary>
    [Column("relationship_status")]
    [StringLength(50)]
    public string RelationshipStatus { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the relationship was removed (optional).
    /// </summary>
    [Column("removed_at")]
    public DateTime? RemovedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who removed this relationship (optional).
    /// </summary>
    [Column("removed_by")]
    public int? RemovedBy { get; set; }

    /// <summary>
    /// Gets or sets the version number for optimistic concurrency control.
    /// </summary>
    [Column("version")]
    public int Version { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the head (parent) account in the relationship.
    /// </summary>
    [Column("head_account_id")]
    public Guid? HeadAccountId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the sub (child) account in the relationship.
    /// </summary>
    [Column("sub_account_id")]
    public Guid? SubAccountId { get; set; }
}
