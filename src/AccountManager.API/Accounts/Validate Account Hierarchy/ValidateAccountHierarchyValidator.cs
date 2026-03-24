// <copyright file="ValidateAccountHierarchyValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Validate_Account_Hierarchy
{
    using FluentValidation;

    /// <summary>
    /// Validator for <see cref="ValidateAccountHierarchyEndpointRequest"/>.
    /// Ensures that both HeadAccountId and SubAccountId are provided in the request.
    /// </summary>
    public class ValidateAccountHierarchyValidator : AbstractValidator<ValidateAccountHierarchyEndpointRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateAccountHierarchyValidator"/> class.
        /// </summary>
        public ValidateAccountHierarchyValidator()
        {
            RuleFor(x => x.HeadAccountId)
                .NotEmpty()
                .WithMessage("HeadAccountId must be provided.");
            RuleFor(x => x.SubAccountId)
                .NotEmpty()
                .WithMessage("SubAccountId must be provided.");
        }
    }
}
