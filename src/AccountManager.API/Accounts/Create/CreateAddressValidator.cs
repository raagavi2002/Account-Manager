// <copyright file="CreateAddressValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Create
{
    using FluentValidation;

    /// <summary>
    /// Validator for <see cref="CreateAddressRequest"/> that enforces
    /// required address fields.
    /// </summary>
    public class CreateAddressValidator
        : AbstractValidator<CreateAddressRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAddressValidator"/> class
        /// and defines validation rules for address creation.
        /// </summary>
        public CreateAddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
        }
    }
}
