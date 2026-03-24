// <copyright file="TimezoneDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Data transfer object representing a timezone reference.
    /// </summary>
    public class TimezoneDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the timezone.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the IANA timezone name (e.g., America/New_York).
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
