// <copyright file="CreateAccountRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Create
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;
    using AccountManager.Domain.ValueObjects;

    /// <summary>
    /// Represents a request to create a new account.
    /// Implements <see cref="IRequirePermission"/> to enforce authorization requirements.
    /// </summary>
    public sealed class CreateAccountRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the account name.</returns>
        required public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        /// <remarks>
        /// This value typically represents a business classification such as
        /// customer, partner, or internal account.
        /// </remarks>
        /// <returns>A <see cref="string"/> representing the account type.</returns>
        required public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets the currency used by the account.
        /// </summary>
        /// <remarks>
        /// Expected to be an ISO 4217 currency code (e.g., EUR, USD).
        /// </remarks>
        /// <returns>A <see cref="string"/> representing the currency code.</returns>
        required public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the timezone associated with the account.
        /// </summary>
        /// <remarks>
        /// Expected to be a valid IANA or system timezone identifier.
        /// </remarks>
        /// <returns>A nullable <see cref="int"/> representing the timezone ID.</returns>
        required public string? Timezone { get; set; }

        /// <summary>
        /// Gets or sets the physical address of the account.
        /// </summary>
        /// <returns>A <see cref="CreateAddressRequest"/> containing address details.</returns>
        required public CreateAddressRequest AddressInfo { get; set; }

        /// <summary>
        /// Gets or sets the VAT number associated with the account.
        /// </summary>
        /// <returns>A nullable <see cref="string"/> containing the VAT number.</returns>
        public string? VatNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account manager responsible for the account.
        /// </summary>
        /// <returns>A nullable <see cref="Guid"/> representing the account manager ID.</returns>
        public Guid? AccountManagerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the customer success manager (CSM) assigned to the account.
        /// </summary>
        /// <returns>A nullable <see cref="Guid"/> representing the CSM ID.</returns>
        public Guid? CsmId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the head or parent account.
        /// </summary>
        /// <returns>A nullable <see cref="Guid"/> representing the head account ID.</returns>
        public Guid? HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the email address used for billing communications.
        /// </summary>
        /// <returns>A nullable <see cref="string"/> containing the billing email address.</returns>
        public string? InvoiceEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the billing type for the account.
        /// </summary>
        /// <returns>A nullable <see cref="string"/> representing the invoice type.</returns>
        public string? InvoiceType { get; set; }

        /// <summary>
        /// Gets or sets the email address used for account notifications.
        /// </summary>
        /// <returns>A nullable <see cref="string"/> containing the notification email address.</returns>
        public string? NotificationEmailAddress { get; set; }

        /// <summary>
        /// Gets the permission required to create an account.
        /// </summary>
        /// <returns>A <see cref="string"/> representing the required permission key.</returns>
        public string RequiredPermission => Permissions.Administrative.Update.AccountName;

        /// <summary>
        /// Gets the account identifier for permission checks.
        /// For new accounts, this is empty until the account is created.
        /// </summary>
        /// <returns>A nullable <see cref="string"/> containing the account ID.</returns>
        public string? AccountId => string.Empty;
    }
}
