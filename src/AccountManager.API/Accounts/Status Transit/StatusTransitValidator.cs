// <copyright file="StatusTransitValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Status_Transit
{
    using FluentValidation;

    /// <summary>
    /// Defines validation rules for <see cref="StatusTransitRequest"/>.
    /// </summary>
    /// <remarks>
    /// Ensures that all required fields for an account status transition
    /// request are provided before processing.
    /// </remarks>
    public class StatusTransitValidator : AbstractValidator<StatusTransitRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusTransitValidator"/> class
        /// and configures validation rules for the status transition request.
        /// </summary>
        public StatusTransitValidator()
        {
           /* RuleFor(x => x.AccountId)
                .NotEmpty();*/

            RuleFor(x => x.AccountStatus)
                .NotEmpty();

           /* RuleFor(x => x.Version)
                .NotEmpty();
*/
            RuleFor(x => x.Reason)
                .NotEmpty();
        }
    }
}
