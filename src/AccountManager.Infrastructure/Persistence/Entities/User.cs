// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a user entity in the Account Manager system.
/// Users belong to accounts and can have multiple roles assigned through UserRole associations.
/// </summary>
[Table("users", Schema = "am")]
[Index("ClerkUserId", Name = "idx_users_clerk_id", IsUnique = true)]
[Index("Email", Name = "idx_users_email", IsUnique = true)]
public partial class User
{
    /// <summary>
    /// Gets or sets the email address of the user. Must be unique across the system.
    /// </summary>
    [Column("email")]
    [StringLength(320)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Clerk authentication service user identifier (optional).
    /// Used for integration with the Clerk authentication platform.
    /// </summary>
    [Column("clerk_user_id")]
    [StringLength(100)]
    public string? ClerkUserId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user account is currently active.
    /// </summary>
    [Column("is_active")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user last logged in (optional).
    /// </summary>
    [Column("last_login_at")]
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Gets or sets the total number of times the user has logged in.
    /// </summary>
    [Column("login_count")]
    public int LoginCount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created this user account.
    /// </summary>
    [Column("created_by")]
    [StringLength(255)]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was last updated.
    /// </summary>
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last updated this user account.
    /// </summary>
    [Column("updated_by")]
    [StringLength(255)]
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was deactivated (optional).
    /// </summary>
    [Column("deactivated_at")]
    public DateTime? DeactivatedAt { get; set; }

    /// <summary>
    /// Gets or sets the version number for optimistic concurrency control.
    /// </summary>
    [Column("version")]
    public int Version { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the account to which this user belongs.
    /// </summary>
    [Column("account_id")]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the account to which this user belongs.
    /// </summary>
    [ForeignKey("AccountId")]
    [InverseProperty("Users")]
    public virtual Account Account { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of role assignments for this user.
    /// </summary>
    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
