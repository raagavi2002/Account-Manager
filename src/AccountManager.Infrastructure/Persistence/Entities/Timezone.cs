// <copyright file="Timezone.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a timezone entity stored in the database.
/// </summary>
[Table("timezone", Schema = "am")]
public partial class Timezone
{
    /// <summary>
    /// Gets or sets the unique identifier for the timezone.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the timezone.
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = null!;
}
