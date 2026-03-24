// <copyright file="AccountStatusChangedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data
{
    /// <summary>
    /// Represents the payload for an account status changed event.
    /// </summary>
    public class AccountStatusChangedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account.
        /// </summary>
        required public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the account name at the time of the status change.
        /// </summary>
        required public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the previous account status.
        /// </summary>
        required public string PreviousStatus { get; set; }

        /// <summary>
        /// Gets or sets the new account status.
        /// </summary>
        required public string NewStatus { get; set; }

        /// <summary>
        /// Gets or sets the logical status transition.
        /// </summary>
        public string? StatusTransition { get; set; }

        /// <summary>
        /// Gets or sets the reason for the status change.
        /// </summary>
        required public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the status changed.
        /// </summary>
        required public DateTime ChangedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that changed the status.
        /// </summary>
        required public string ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the account version after the status change.
        /// </summary>
        required public int Version { get; set; }
    }
}
