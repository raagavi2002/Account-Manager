// <copyright file="GetAccountDetailsQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetAccountDetailsQuery
{
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="GetAccountDetailsQueryRequest"/> query by retrieving account details
    /// from the repository and mapping them to a <see cref="GetAccountDetailsQueryResponse"/>.
    /// </summary>
    public class GetAccountDetailsQueryHandler : IRequestHandler<GetAccountDetailsQueryRequest, GetAccountDetailsQueryResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IApplogger applogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountDetailsQueryHandler"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository used to access account data.</param>
        /// <param name="applogger">The application logger used for logging operations.</param>
        public GetAccountDetailsQueryHandler(IAccountRepository accountRepository, IApplogger applogger)
        {
            this.accountRepository = accountRepository;
            this.applogger = applogger;
        }

        /// <summary>
        /// Handles the request to retrieve account details by account ID.
        /// </summary>
        /// <param name="request">The request containing the account ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// the <see cref="GetAccountDetailsQueryResponse"/> with account details.
        /// </returns>
        public async Task<GetAccountDetailsQueryResponse> Handle(GetAccountDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var accountInfo = await accountRepository.GetAccountInfoByIdAsync(request.AccountId);

            return new GetAccountDetailsQueryResponse
            {
                AccountId = accountInfo?.AccountId,
                AccountName = accountInfo?.AccountName,
                AccountType = accountInfo?.AccountType,
                Currency = accountInfo?.Currency,
                Timezone = accountInfo?.Timezone,
                Address = accountInfo?.Address,
                VatNumber = accountInfo?.VatNumber,
                AccountManagerId = accountInfo?.AccountManagerId,
                CsmId = accountInfo?.CsmId,
                HeadAccountId = accountInfo?.HeadAccountId,
                InvoiceEmailAddress = accountInfo?.InvoiceEmailAddress,
                InvoiceType = accountInfo?.InvoiceType,
                NotificationEmailAddress = accountInfo?.NotificationEmailAddress,
                AccountStatus = accountInfo?.AccountStatus,
                Version = accountInfo?.Version ?? 0,
                IsHeadAccount = accountInfo?.IsHeadAccount,
                CreatedAt = accountInfo?.CreatedAt,
                UpdatedAt = accountInfo?.UpdatedAt,
                ActivatedAt = accountInfo?.ActivatedAt,
                DeactivatedAt = accountInfo?.DeactivatedAt,
            };
        }
    }
}
