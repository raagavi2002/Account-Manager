// <copyright file="GetAccountProductsValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountProducts
{
    using FluentValidation;

    /// <summary>
    /// Validates account-products query parameters.
    /// </summary>
    public class GetAccountProductsValidator : AbstractValidator<GetAccountProductsAPIRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountProductsValidator"/> class.
        /// </summary>
        public GetAccountProductsValidator()
        {
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("pageSize must be between 1 and 100.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("pageNumber must be greater than or equal to 1.");
        }
    }
}
