// <copyright file="GetTimezoneQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetTimezoneQuery
{
    using AccountManager.Domain.Interfaces;
    using MediatR;

    /// <summary>
    /// Handles the retrieval of all available timezones.
    /// </summary>
    public class GetTimezoneQueryHandler : IRequestHandler<GetTimezoneQueryRequest, GetTimezoneQueryResponse>
    {
        private readonly IAccountRepository accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTimezoneQueryHandler"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public GetTimezoneQueryHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Handles the request to get all timezones.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A response containing the list of timezones.</returns>
        public async Task<GetTimezoneQueryResponse> Handle(GetTimezoneQueryRequest request, CancellationToken cancellationToken)
        {
            var timezones = await accountRepository.GetAllTimezonesAsync().ConfigureAwait(false);
            return new GetTimezoneQueryResponse
            {
                Timezones = timezones,
            };
        }
    }
}
