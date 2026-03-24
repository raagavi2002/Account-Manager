// <copyright file="StatusTransitEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.StatusTransit
{
    using AccountManager.API.Accounts.Create;
    using AccountManager.API.Accounts.Status_Transit;
    using AccountManager.API.Authorization;
    using AccountManager.API.ErrorResponses;
    using AccountManager.Application.Commands.AccountStatusTransitCommand;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Exceptions;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// Endpoint responsible for handling account status transition requests.
    /// </summary>
    /// <remarks>
    /// This endpoint allows changing the status of an existing account by issuing
    /// a status transition command through MediatR.
    /// </remarks>
    public class StatusTransitEndpoint : Endpoint<StatusTransitRequest, StatusTransitResponse>
    {
        private readonly IMediator mediator;
        private readonly AutoMapper.IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusTransitEndpoint"/> class.
        /// </summary>
        /// <param name="mediator">Mediator used to dispatch commands.</param>
        /// <param name="mapper">Mapper used to convert between API models and DTOs.</param>
        public StatusTransitEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Configures the endpoint route, versioning, authorization, and validation.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Put("/accounts/{AccountId}/status");
            AllowAnonymous();
            PreProcessor<PermissionPreProcessor<StatusTransitRequest>>();
            Validator<StatusTransitValidator>();
            Summary(s =>
            {
                s.Summary = "Transitions the status of an existing account.";
                s.Description = "This endpoint allows changing the status of an existing account by issuing a status transition command through MediatR.";
                s.Response<StatusTransitResponse>(200, "Status transition successful.");
            });
        }

        /// <summary>
        /// Handles the account status transition request.
        /// </summary>
        /// <param name="request">The status transition request payload.</param>
        /// <param name="cancellationToken">Token used to cancel the request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="AccountValidationException">
        /// Thrown when the request fails validation or required conditions are not met.
        /// </exception>
        /// <exception cref="InvalidAccountStatusTransitionException">
        /// Thrown when the requested status transition is invalid or redundant.
        /// </exception>
        /// <exception cref="AccountAlreadyExistsException">
        /// Thrown when the transition conflicts with existing account state.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the account does not exist.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the caller lacks sufficient permissions.
        /// </exception>
        public override async Task HandleAsync(
            StatusTransitRequest request,
            CancellationToken cancellationToken)
        {
                // Retrieve account identifier from route
                var accId = Route<Guid>("accountId");

                // Map request to domain DTO
                var statusTransitDto = mapper.Map<AccountStatusTransitDto>(request);
                statusTransitDto.AccountId = accId;

                // Create and dispatch command
                var command = new AccountStatusTransitCommand
                {
                    AccountStatusTransitInfo = statusTransitDto,
                };

                var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

                // Map result to response
                var response = mapper.Map<StatusTransitResponse>(result);

                // Return 200 OK for successful status transition
                await Send.OkAsync(response, cancellationToken).ConfigureAwait(false);
        }
    }
}
