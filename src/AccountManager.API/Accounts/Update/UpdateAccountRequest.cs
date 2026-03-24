// <copyright file="UpdateAccountRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Update
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums.Authorization;

    /// <summary>
    /// Represents a request to update an account with various account details.
    /// </summary>
    public class UpdateAccountRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account.
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the account name.
        /// </summary>
        public string? AccountName { get; set; }

        /// <summary>
        /// Gets or sets the account type.
        /// </summary>
        public string? AccountType { get; set; }

        /// <summary>
        /// Gets or sets the account currency code (e.g., USD, EUR).
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the time zone identifier.
        /// </summary>
        public string? Timezone { get; set; }

        /// <summary>
        /// Gets or sets the primary address associated with the account.
        /// </summary>
        public AddressDto? Address { get; set; }

        /// <summary>
        /// Gets or sets the VAT number.
        /// </summary>
        public string? VatNumber { get; set; }

        /// <summary>
        /// Gets or sets the account manager identifier.
        /// </summary>
        public int? AccountManagerId { get; set; }

        /// <summary>
        /// Gets or sets the customer success manager identifier.
        /// </summary>
        public int? CsmId { get; set; }

        /// <summary>
        /// Gets or sets the parent (head) account identifier.
        /// </summary>
        public Guid? HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the email address used for billing communications.
        /// </summary>
        public string? InvoiceEmail { get; set; }

        /// <summary>
        /// Gets or sets the billing type for the account.
        /// </summary>
        public string? InvoiceType { get; set; }

        /// <summary>
        /// Gets or sets the email address used for account notifications.
        /// </summary>
        public string? NotificationEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the current version of the account entity.
        /// Used for optimistic concurrency control.
        /// </summary>
        public int Version { get; set; }

        public string RequiredPermission => Permissions.Administrative.Update.Account;

        string? IRequirePermission.AccountId => AccountId?.ToString();
    }
}
