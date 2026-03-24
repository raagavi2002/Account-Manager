// <copyright file="UpdateAccountResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Update
{
    /// <summary>
    /// Represents a response to update an account with various account details.
    /// </summary>
    public class UpdateAccountResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account that was updated.
        /// </summary>
        public int AccountId { get; set; }

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
