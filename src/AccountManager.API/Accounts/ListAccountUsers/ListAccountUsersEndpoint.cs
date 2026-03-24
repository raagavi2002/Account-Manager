// <copyright file="ListAccountUsersEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.ListAccountUsers
{
    using AccountManager.API.Authorization;
    using AccountManager.Application.Queries.ListAccountUsersQuery;
    using AccountManager.Domain.Exceptions;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// Endpoint for retrieving users associated with an account.
    /// </summary>
    public class ListAccountUsersEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        : Endpoint<ListAccountUsersAPIRequest, ListAccountUsersAPIResponse>
    {
        /// <summary>
        /// Configures endpoint metadata.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Get("/accounts/{accountId}/users");
            AllowAnonymous(); // Validated by preprocessor.

            Summary(s =>
            {
                s.Summary = "List Account Users";
                s.Description = "Retrieves paginated users associated with a specific account.";
                s.Response<ListAccountUsersAPIResponse>(StatusCodes.Status200OK, "Successfully retrieved account users.");
                s.Response<AccountNotFoundException>(StatusCodes.Status404NotFound, "Account not found.");
            });
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="req">The request object.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task HandleAsync(ListAccountUsersAPIRequest req, CancellationToken ct)
        {
            req.AccountId = Route<Guid>("accountId");
            var request = mapper.Map<ListAccountUsersQueryRequest>(req);
            var result = await mediator.Send(request, ct).ConfigureAwait(false);
            var response = mapper.Map<ListAccountUsersAPIResponse>(result);
            await Send.OkAsync(response, ct).ConfigureAwait(false);
        }
    }
}
