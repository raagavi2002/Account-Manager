// <copyright file="GetAccountDetailsEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountDetails
{
    using AccountManager.Application.Queries.GetAccountDetailsQuery;
    using AccountManager.Domain.Exceptions;
    using AccountManager.API.Authorization;
    using AutoMapper;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// Endpoint for handling HTTP GET requests to retrieve detailed information about a specific account.
    /// </summary>
    /// <param name="mediator">The <see cref="IMediator"/> instance used to send application queries.</param>
    public class GetAccountDetailsEndpoint(IMediator mediator, AutoMapper.IMapper mapper) : Endpoint<GetAccountDetailsAPIRequest, GetAccountDetailsAPIResponse>
    {
        /// <summary>
        /// Handles the HTTP GET request to retrieve detailed information about a specific account by its identifier.
        /// </summary>
        /// <param name="ct">A cancellation token.</param>
        public override async Task HandleAsync(GetAccountDetailsAPIRequest req, CancellationToken ct)
        {
            // Retrieve the accountId from the route
            req.AccountId = Route<Guid>("accountId");
            GetAccountDetailsQueryRequest request = mapper.Map<GetAccountDetailsQueryRequest>(req);
            var commandResponse = await mediator.Send(request, ct);
            GetAccountDetailsAPIResponse response = mapper.Map<GetAccountDetailsAPIResponse>(commandResponse);
            await Send.OkAsync(response, ct);
        }

        /// <summary>
        /// Configures the endpoint for retrieving account details.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Get("/accounts/{accountId}");
            AllowAnonymous(); // Validated by PreProcessor
            Summary(s =>
            {
                s.Summary = "Get Account Details";
                s.Description = "Retrieves detailed information about a specific account by its identifier.";
                s.Response<GetAccountDetailsAPIResponse>(StatusCodes.Status200OK, "Successfully retrieved account details.");
                s.Response<AccountNotFoundException>(StatusCodes.Status404NotFound, "Account not found.");
            });
        }
    }
}
