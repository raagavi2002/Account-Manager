// <copyright file="UnlinkSubAccountValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts
{
    using AccountManager.API.Accounts.Unlink_Sub_Account;
    using FluentValidation;

    /// <summary>
    /// Validator for <see cref="UnlinkSubAccountEndpointRequest"/>.
    /// Ensures all required properties are provided and valid.
    /// </summary>
    public class UnlinkSubAccountValidator : AbstractValidator<UnlinkSubAccountEndpointRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnlinkSubAccountValidator"/> class.
        /// </summary>
        public UnlinkSubAccountValidator()
        {
            RuleFor(x => x.HeadAccountId)
                .NotEmpty()
                .WithMessage("HeadAccountId must be provided.");

            RuleFor(x => x.SubAccountId)
                .NotEmpty()
                .WithMessage("SubAccountId must be provided.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason must be provided.")
                .MaximumLength(500)
                .WithMessage("Reason cannot exceed 500 characters.");
        }
    }
}
