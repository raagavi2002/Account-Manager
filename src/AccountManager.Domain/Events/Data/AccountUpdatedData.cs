// <copyright file="AccountUpdatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data
{
    /// <summary>
    /// Represents the payload for an account updated event.
    /// </summary>
    public class AccountUpdatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account.
        /// </summary>
        required public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the account name at the time of the update.
        /// </summary>
        required public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the collection of fields that were changed.
        /// </summary>
        required public List<FieldChange> ChangedFields { get; set; }

        /// <summary>
        /// Gets or sets the type of change performed.
        /// </summary>
        required public string ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the reason for the change.
        /// </summary>
        required public string ChangeReason { get; set; }

        /// <summary>
        /// Gets or sets the version of the account after the update.
        /// </summary>
        required public int Version { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the account was updated.
        /// </summary>
        required public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that performed the update.
        /// </summary>
        required public string UpdatedBy { get; set; }
    }
}
