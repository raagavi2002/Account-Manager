// <copyright file="ArchiveAccountValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Archive
{
    using FluentValidation;

    /// <summary>
    /// Validator for <see cref="ArchiveAccountEndpointRequest"/>.
    /// Ensures all required properties are provided and valid.
    /// </summary>
    public class ArchiveAccountValidator : AbstractValidator<ArchiveAccountEndpointRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveAccountValidator"/> class.
        /// </summary>
        public ArchiveAccountValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("AccountId must be provided.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason must be provided.")
                .MaximumLength(500)
                .WithMessage("Reason cannot exceed 500 characters.");
        }
    }
}
