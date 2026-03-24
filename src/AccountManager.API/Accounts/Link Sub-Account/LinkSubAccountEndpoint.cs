// <copyright file="LinkSubAccountEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Link_Sub_Account
{
    using AccountManager.API.Authorization;
    using AccountManager.Application.Commands.LinkSubAccountCommand;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Shared.Logging;
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Endpoint for linking a sub-account to a head account.
    /// </summary>
    /// <remarks>
    /// This endpoint creates a head–sub account relationship between two existing accounts.
    /// It validates that both accounts exist, ensures the sub-account is not already linked,
    /// and prevents circular or multi-level hierarchies.
    /// </remarks>
    public class LinkSubAccountEndpoint(IMediator mediator, AutoMapper.IMapper mapper, IApplogger applogger) : Endpoint<LinkSubAccountRequest, LinkSubAccountResponse>
    {
        /// <summary>
        /// Configures the HTTP route and behavior for the link sub-account endpoint.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Post("/accounts/{headAccountId}/relationships");
            AllowAnonymous();
            PreProcessor<PermissionPreProcessor<LinkSubAccountRequest>>();
            Summary(s =>
            {
                s.Summary = "Link Sub-Account to Head Account";
                s.Description = "Creates a head–sub account relationship between two existing accounts.";
                s.Response<LinkSubAccountResponse>(StatusCodes.Status201Created, "Successfully linked sub-account to head account.");
                s.Response<ErrorResponse>(StatusCodes.Status400BadRequest, "Invalid request data.");
                s.Response<ErrorResponse>(StatusCodes.Status404NotFound, "Head account or sub-account not found.");
                s.Response<ErrorResponse>(StatusCodes.Status409Conflict, "Sub-account is already linked or linking would create a circular relationship.");
            });
        }

        /// <summary>
        /// Handles the request to link a sub-account to a head account.
        /// </summary>
        /// <param name="req">
        /// The request containing the sub-account identifier and relationship details.
        /// The head account identifier is resolved from the route.
        /// </param>
        /// <param name="ct">
        /// A cancellation token used to propagate notification that the operation should be canceled.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation that returns
        /// a <see cref="LinkSubAccountResponse"/> upon successful creation.
        /// </returns>
        public override async Task HandleAsync(LinkSubAccountRequest req, CancellationToken ct)
        {
                // Resolve head account ID from route
                req.HeadAccountId = Route<Guid>("headAccountId");

                // Map request → DTO
                LinkSubAccountDto dto = mapper.Map<LinkSubAccountDto>(req);

                var command = new LinkSubAccountCommand
                {
                    LinkSubAccountDto = dto,
                };

                // Execute command
                var result = await mediator.Send(command, ct).ConfigureAwait(false);

                // Map command response → API response
                var response = mapper.Map<LinkSubAccountResponse>(result);

                // Return 201 Created
                await Send.ResponseAsync(response, (int)StatusCodes.Status201Created, ct).ConfigureAwait(false);
        }
    }
}
