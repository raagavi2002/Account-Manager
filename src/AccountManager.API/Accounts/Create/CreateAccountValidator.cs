// <copyright file="CreateAccountValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Create
{
    using FluentValidation;

    /// <summary>
    /// Validator for <see cref="CreateAccountRequest"/> that enforces
    /// required fields for account creation.
    /// </summary>
    public class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountValidator"/> class
        /// and defines validation rules for account creation.
        /// </summary>
        public CreateAccountValidator()
        {
            // Validates that the account name is provided.
            RuleFor(x => x.AccountName)
                .NotEmpty();

            // Validates that the account type is provided.
            RuleFor(x => x.AccountType)
                .NotEmpty();

            // Validates that the currency is provided.
            RuleFor(x => x.Currency)
                .NotEmpty();

            // Validates that the timezone is provided.
            RuleFor(x => x.Timezone)
                .NotEmpty();

            // Validates that the VAT number is provided.
            RuleFor(x => x.VatNumber)
                .NotEmpty();

            RuleFor(x => x.NotificationEmailAddress)
             .EmailAddress()
             .Unless(x => string.IsNullOrWhiteSpace(x.NotificationEmailAddress));

            RuleFor(x => x.InvoiceEmailAddress)
                .EmailAddress()
                .Unless(x => string.IsNullOrWhiteSpace(x.InvoiceEmailAddress));

            // Validates that address information is provided and
            // applies nested address validation rules.
            RuleFor(x => x.AddressInfo)
                .NotNull()
                .SetValidator(new CreateAddressValidator());
        }
    }
}
