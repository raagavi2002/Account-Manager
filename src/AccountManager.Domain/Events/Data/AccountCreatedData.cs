// <copyright file="AccountCreatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data
{
    /// <summary>
    /// Represents the payload for an account created event.
    /// </summary>
    public class AccountCreatedData
    {
        /// <summary>
        /// Gets or sets the created account details.
        /// </summary>
        public AccountData? Account { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that created the account.
        /// </summary>
        public string? CreatedBy { get; set; }
    }
}
