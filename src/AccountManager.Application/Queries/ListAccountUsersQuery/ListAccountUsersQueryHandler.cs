// <copyright file="ListAccountUsersQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.ListAccountUsersQuery
{
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles <see cref="ListAccountUsersQueryRequest"/> queries.
    /// </summary>
    public class ListAccountUsersQueryHandler : IRequestHandler<ListAccountUsersQueryRequest, ListAccountUsersQueryResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IApplogger applogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListAccountUsersQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="applogger">The application logger.</param>
        public ListAccountUsersQueryHandler(IUserRepository userRepository, IApplogger applogger)
        {
            this.userRepository = userRepository;
            this.applogger = applogger;
        }

        /// <inheritdoc/>
        public async Task<ListAccountUsersQueryResponse> Handle(ListAccountUsersQueryRequest request, CancellationToken cancellationToken)
        {
            int pageSize = request.PageSize <= 0 ? 20 : Math.Min(request.PageSize, 100);
            int pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;

            var (users, totalCount) = await userRepository.GetAccountUsersAsync(
                request.AccountId,
                request.IsActive,
                request.Role,
                pageSize,
                pageNumber,
                cancellationToken).ConfigureAwait(false);

            applogger.LogInformation(
                "Retrieved account users",
                new
                {
                    UserCount = users.Count,
                    request.AccountId,
                    totalCount,
                    pageNumber,
                    pageSize,
                });

            return new ListAccountUsersQueryResponse
            {
                Users = users,
                TotalCount = totalCount,
                Page = pageNumber,
                PageSize = pageSize,
                HasMore = totalCount > (pageNumber * pageSize),
            };
        }
    }
}
