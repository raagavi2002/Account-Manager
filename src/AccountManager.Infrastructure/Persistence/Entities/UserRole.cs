// <copyright file="UserRole.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the association between a user and a role, implementing a many-to-many relationship.
/// Supports temporal role assignments with effective date ranges.
/// </summary>
[Table("user_roles", Schema = "am")]
[Index("RoleName", Name = "idx_user_roles_role")]
public partial class UserRole
{
    /// <summary>
    /// Gets or sets the unique identifier for the user-role assignment.
    /// </summary>
    [Key]
    [Column("user_role_id")]
    public int UserRoleId { get; set; }

    /// <summary>
    /// Gets or sets the name of the role assigned to the user.
    /// </summary>
    [Column("role_name")]
    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the role assignment becomes effective.
    /// </summary>
    [Column("effective_from")]
    public DateTime EffectiveFrom { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the role assignment expires (optional).
    /// If null, the role assignment has no expiration date.
    /// </summary>
    [Column("effective_to")]
    public DateTime? EffectiveTo { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who assigned this role.
    /// </summary>
    [Column("assigned_by")]
    public int AssignedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the role assignment was created.
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the role being assigned.
    /// </summary>
    [Column("role_id")]
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the numeric type identifier for the role, used for categorization.
    /// </summary>
    [Column("role_type")]
    public int RoleType { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user to whom the role is assigned.
    /// </summary>
    [Column("user_id")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the role entity associated with this assignment.
    /// </summary>
    [ForeignKey("RoleId")]
    [InverseProperty("UserRoles")]
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user entity associated with this role assignment.
    /// </summary>
    [ForeignKey("UserId")]
    [InverseProperty("UserRoles")]
    public virtual User? User { get; set; }
}
