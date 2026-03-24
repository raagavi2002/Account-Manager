// <copyright file="UpdateAccountResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Results
{
    using System;

    /// <summary>
    /// Represents the result of an account update operation.
    /// </summary>
    public class UpdateAccountResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account that was updated.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the current version of the account after the update.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who performed the update.
        /// </summary>
        public int UpdatedBy { get; set; }
    }
}
