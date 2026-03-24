// <copyright file="GetAccountProductsQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetAccountProductsQuery
{
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles <see cref="GetAccountProductsQueryRequest"/> queries.
    /// </summary>
    public class GetAccountProductsQueryHandler : IRequestHandler<GetAccountProductsQueryRequest, GetAccountProductsQueryResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IApplogger applogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountProductsQueryHandler"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="applogger">The application logger.</param>
        public GetAccountProductsQueryHandler(IAccountRepository accountRepository, IApplogger applogger)
        {
            this.accountRepository = accountRepository;
            this.applogger = applogger;
        }

        /// <inheritdoc/>
        public async Task<GetAccountProductsQueryResponse> Handle(GetAccountProductsQueryRequest request, CancellationToken cancellationToken)
        {
            int pageSize = request.PageSize <= 0 ? 20 : Math.Min(request.PageSize, 100);
            int pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;

            var (products, totalCount) = await accountRepository.GetAccountProductsAsync(
                request.AccountId,
                request.IsActive,
                pageSize,
                pageNumber).ConfigureAwait(false);

            applogger.LogInformation(
                "Retrieved account products",
                new
                {
                    ProductCount = products.Count,
                    request.AccountId,
                    totalCount,
                    pageNumber,
                    pageSize,
                });

            return new GetAccountProductsQueryResponse
            {
                Products = products,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                HasMore = totalCount > (pageNumber * pageSize),
            };
        }
    }
}
