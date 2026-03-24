// <copyright file="AccountLinkedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents the payload for an account linked event.
    /// </summary>
    public class AccountLinkedData
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
        public DateTime LinkedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that performed the linking.
        /// </summary>
        required public string LinkedBy { get; set; }
    }
}
