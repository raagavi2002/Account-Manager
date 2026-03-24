// <copyright file="Role.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a role that defines permissions and access levels within the Account Manager system.
/// </summary>
[Table("roles", Schema = "am")]
public partial class Role
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the numeric type identifier for the role, used for categorization or hierarchical organization.
    /// </summary>
    [Column("role_type")]
    public int RoleType { get; set; }

    /// <summary>
    /// Gets or sets the name of the role (e.g., Administrator, User, Manager).
    /// </summary>
    [Column("role_name")]
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of user-role assignments associated with this role.
    /// </summary>
    [InverseProperty("Role")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
