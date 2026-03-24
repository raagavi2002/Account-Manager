// <copyright file="UserUpdatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents the payload for a user updated event.
    /// </summary>
    public class UserUpdatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account the user belongs to.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the collection of changed fields, keyed by field name.
        /// </summary>
        required public Dictionary<string, UserFieldChange> ChangedFields { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the user was updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that performed the update.
        /// </summary>
        required public string UpdatedBy { get; set; }
    }
}
