// <copyright file="CreateAccountDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Represents account-related data used for data transfer.
    /// </summary>
    public class CreateAccountDto
    {
        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        required public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        /// <remarks>
        /// This value typically represents a business classification such as
        /// customer, partner, or internal account.
        /// </remarks>
        required public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets the currency used by the account.
        /// </summary>
        /// <remarks>
        /// Expected to be an ISO 4217 currency code (e.g. EUR, USD).
        /// </remarks>
        required public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the timezone associated with the account.
        /// </summary>
        /// <remarks>
        /// Expected to be a valid IANA or system timezone identifier.
        /// </remarks>
        required public string? Timezone { get; set; }

        /// <summary>
        /// Gets or sets the physical address of the account.
        /// </summary>
        required public AddressDto Address { get; set; }

        /// <summary>
        /// Gets or sets the VAT number associated with the account.
        /// </summary>
        public string? VatNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account manager responsible for the account.
        /// </summary>
        public Guid? AccountManagerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the customer success manager (CSM) assigned to the account.
        /// </summary>
        public Guid? CsmId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the head or parent account.
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
    }
}
