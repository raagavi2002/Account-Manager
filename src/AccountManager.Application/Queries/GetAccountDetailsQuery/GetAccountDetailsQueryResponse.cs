// <copyright file="GetAccountDetailsResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetAccountDetailsQuery
{
    using AccountManager.Domain.DTO;

    /// <summary>
    /// Represents the response containing detailed information about an account.
    /// </summary>
    public class GetAccountDetailsQueryResponse
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
        public Guid? AccountManagerId { get; set; }

        /// <summary>
        /// Gets or sets the customer success manager identifier.
        /// </summary>
        public Guid? CsmId { get; set; }

        /// <summary>
        /// Gets or sets the parent (head) account identifier.
        /// </summary>
        public Guid? HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the email address used for billing communications.
        /// </summary>
        public string? InvoiceEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the billing type for the account.
        /// </summary>
        public string? InvoiceType { get; set; }

        /// <summary>
        /// Gets or sets the email address used for account notifications.
        /// </summary>
        public string? NotificationEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the status of the account.
        /// </summary>
        public string? AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets the current version of the account entity.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this account is a head (parent) account.
        /// </summary>
        public bool? IsHeadAccount { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was activated.
        /// </summary>
        public DateTime? ActivatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was deactivated.
        /// </summary>
        public DateTime? DeactivatedAt { get; set; }
    }
}
