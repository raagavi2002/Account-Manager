// <copyright file="AccountData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data
{
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.ValueObjects;

    /// <summary>
    /// Represents core account information used in account-related events.
    /// </summary>
    public class AccountData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the account.
        /// </summary>
        required public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        required public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets the default currency associated with the account.
        /// </summary>
        required public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the time zone identifier for the account.
        /// </summary>
        required public string TimezoneId { get; set; }

        /// <summary>
        /// Gets or sets the current status of the account.
        /// </summary>
        required public string Status { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the account.
        /// </summary>
        required public AddressDto Address { get; set; }

        /// <summary>
        /// Gets or sets the VAT or tax identification number.
        /// </summary>
        required public string VatNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account manager.
        /// </summary>
        required public Guid? AccountManagerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the customer success manager.
        /// </summary>
        required public Guid? CsmId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this account is a head account.
        /// </summary>
        required public bool IsHeadAccount { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the head account, if applicable.
        /// </summary>
        required public Guid? HeadAccountId { get; set; }

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
    }
}
