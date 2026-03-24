// <copyright file="ValidateAccountHierarchyCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.ValidateAccountHierarchyCommand
{
    using AccountManager.Application.Abstractions;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Errors;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="ValidateAccountHierarchyCommand"/> request by validating
    /// the relationship between a head account and a sub account.
    /// </summary>
    /// <remarks>
    /// This handler checks for account existence, ensures that the sub account is not already linked,
    /// and returns a <see cref="ValidateAccountHierarchyResponse"/> with validation results.
    /// </remarks>
    public class ValidateAccountHierarchyCommandHandler
        : IRequestHandler<ValidateAccountHierarchyCommand, ValidateAccountHierarchyResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IApplogger applogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateAccountHierarchyCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work providing access to account repositories.</param>
        /// <param name="applogger">The application logger for recording validation events.</param>
        public ValidateAccountHierarchyCommandHandler(IUnitOfWork unitOfWork, IApplogger applogger)
        {
            this.unitOfWork = unitOfWork;
            this.applogger = applogger;
        }

        /// <summary>
        /// Handles the validation of account hierarchy.
        /// </summary>
        /// <param name="request">The command containing head and sub account identifiers.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A <see cref="ValidateAccountHierarchyResponse"/> indicating whether the hierarchy is valid,
        /// along with validation messages and account details when applicable.
        /// </returns>
        public async Task<ValidateAccountHierarchyResponse> Handle(
            ValidateAccountHierarchyCommand request,
            CancellationToken cancellationToken)
        {
            if (!await unitOfWork.Accounts.CheckAccountExistsAsync(request.HeadAccountId))
            {
                return new ValidateAccountHierarchyResponse()
                {
                    IsValid = false,
                    ValidationMessages = new List<string>()
                    {
                        "Head Account does not exist",
                    },
                };
            }

            // Get sub-account info
            var subAccountInfo = await unitOfWork.Accounts
                .GetAccountInfoByIdAsync(request.SubAccountId)
                .ConfigureAwait(false);

            if (subAccountInfo == null)
            {
                return new ValidateAccountHierarchyResponse()
                {
                    IsValid = false,
                    ValidationMessages = new List<string>()
                    {
                        "Sub Account does not exist",
                    },
                    HeadAccountInfo = new AccountDto
                    {
                        AccountId = request.HeadAccountId,
                    },
                };
            }

            // Check if already linked
            if (subAccountInfo.HeadAccountId.ToString() == request.HeadAccountId.ToString())
            {
                return new ValidateAccountHierarchyResponse()
                {
                    IsValid = false,
                    ValidationMessages = new List<string>()
                    {
                        "Account has already been linked",
                    },
                    HeadAccountInfo = new AccountDto
                    {
                        AccountId = request.HeadAccountId,
                    },
                    SubAccountInfo = new AccountDto
                    {
                        AccountId = request.SubAccountId,
                    },
                };
            }

            if (!string.IsNullOrEmpty(subAccountInfo.HeadAccountId.ToString()))
            {
                return new ValidateAccountHierarchyResponse()
                {
                    IsValid = false,
                    ValidationMessages = new List<string>()
                    {
                        "Account has already been linked with another head account",
                    },
                    HeadAccountInfo = new AccountDto
                    {
                        AccountId = request.HeadAccountId,
                    },
                    SubAccountInfo = new AccountDto
                    {
                        AccountId = request.SubAccountId,
                    },
                };
            }

            return new ValidateAccountHierarchyResponse()
            {
                IsValid = true,
                ValidationMessages = new List<string>(),
            };
        }
    }
}
