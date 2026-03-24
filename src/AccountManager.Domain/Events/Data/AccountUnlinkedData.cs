// <copyright file="AccountUnlinkedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data
{
    /// <summary>
    /// Represents the payload for an account unlinked event.
    /// </summary>
    public class AccountUnlinkedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sub-account.
        /// </summary>
        public Guid SubAccountId { get; set; }

        /// <summary>
        /// Gets or sets the name of the sub-account.
        /// </summary>
        required public string SubAccountName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the head account.
        /// </summary>
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the name of the head account.
        /// </summary>
        public string? HeadAccountName { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the accounts were linked.
        /// </summary>
        public DateTime UnlinkedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that performed the linking.
        /// </summary>
        required public string UnlinkedBy { get; set; }

        /// <summary>
        /// Gets or sets the reason for unlinking the accounts.
        /// </summary>
        required public string Reason { get; set; }
    }
}
