// <copyright file="AccountRelationshipDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.DTO
{
    using System;

    /// <summary>
    /// Data Transfer Object representing a hierarchical relationship between a head account and a sub-account.
    /// </summary>
    public class AccountRelationshipDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the account relationship.
        /// </summary>
        public int AccountRelationshipId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who established this relationship.
        /// </summary>
        public int EstablishedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the relationship was established.
        /// </summary>
        public DateTime EstablishedAt { get; set; }

        /// <summary>
        /// Gets or sets the current status of the relationship (e.g., ACTIVE, INACTIVE).
        /// </summary>
        public string RelationshipStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the relationship was removed (optional).
        /// </summary>
        public DateTime? RemovedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who removed this relationship (optional).
        /// </summary>
        public int? RemovedBy { get; set; }

        /// <summary>
        /// Gets or sets the version number for optimistic concurrency control.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the head (parent) account in the relationship.
        /// </summary>
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the sub (child) account in the relationship.
        /// </summary>
        public Guid SubAccountId { get; set; }
    }
}
