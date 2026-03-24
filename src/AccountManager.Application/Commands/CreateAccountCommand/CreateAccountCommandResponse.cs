// <copyright file="CreateAccountCommandResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.CreateAccountCommand
{
    /// <summary>
    /// Represents the response returned after successfully creating an account.
    /// </summary>
    public class CreateAccountCommandResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the created account.
        /// </summary>
        required public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        required public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        required public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets the current status of the account.
        /// </summary>
        /// <remarks>
        /// Examples include Active, Inactive, or Pending.
        /// </remarks>
        required public string AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets the currency associated with the account.
        /// </summary>
        /// <remarks>
        /// Expected to be an ISO 4217 currency code (e.g. EUR, USD).
        /// </remarks>
        required public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the timezone associated with the account.
        /// </summary>
        required public string Timezone { get; set; }

        /// <summary>
        /// Gets or sets the current version of the account record.
        /// </summary>
        required public int Version { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was created.
        /// </summary>
        required public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was last updated.
        /// </summary>
        required public DateTime UpdatedAt { get; set; }
    }
}
