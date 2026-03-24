// <copyright file="AddUserValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.Add_User
{
    using FluentValidation;

    /// <summary>
    /// Validator class for <see cref="AddUserEndpointRequest"/>.
    /// Ensures that required fields are provided and meet format requirements.
    /// </summary>
    public class AddUserValidator : AbstractValidator<AddUserEndpointRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserValidator"/> class.
        /// Defines validation rules for adding a user.
        /// </summary>
        public AddUserValidator()
        {
            /// <summary>
            /// Rule: AccountId must not be empty.
            /// </summary>
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("AccountId is required.");

            /// <summary>
            /// Rule: Email must not be empty and must be a valid email address.
            /// </summary>
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");
        }
    }
}
