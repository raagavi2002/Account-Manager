// <copyright file="ArchiveAccountEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Archive
{
    using AccountManager.API.Authorization;
    using AccountManager.Application.Commands.ArchiveAccountCommand;
    using AccountManager.Domain.DTO;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// API endpoint for archiving an account.
    /// Handles incoming requests, maps them to domain DTOs,
    /// executes the archive command via MediatR, and returns the response.
    /// </summary>
    /// <param name="mapper"> Provides object-to-object mapping between request/response models and DTOs </param>
    /// <param name="mediator"> Mediator for sending commands and receiving responses in a decoupled manner.
    /// </param>
    internal sealed class ArchiveAccountEndpoint(AutoMapper.IMapper mapper, IMediator mediator) : Endpoint<ArchiveAccountEndpointRequest, ArchiveAccountEndpointResponse>
    {
        /// <summary>
        /// Configures the endpoint route, version, and summary.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Post("/accounts/{accountId}/archive");
            AllowAnonymous(); // Validated by PreProcessor
            PreProcessor<PermissionPreProcessor<ArchiveAccountEndpointRequest>>();
            Summary(s => new ArchiveAccountEndpointResponse
            {
                AccountId = Guid.NewGuid(),
                Reason = "Contract ended",
                IsArchived = true,
                IsGDPRComplaint = true,
                ArchivedAt = DateTime.UtcNow,
                ArchivedBy = "system",
            });
        }

        /// <summary>
        /// Handles the archive account request by mapping the request to a DTO,
        /// sending the command via MediatR, and returning the mapped response.
        /// </summary>
        /// <param name="archiveAccountEndpointRequest">The incoming API request to archive an account.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task HandleAsync(ArchiveAccountEndpointRequest archiveAccountEndpointRequest, CancellationToken cancellationToken)
        {
            var accId = Route<Guid>("accountId");
            ArchiveAccountDto archiveAccountDto = mapper.Map<ArchiveAccountDto>(archiveAccountEndpointRequest);
            archiveAccountDto.AccountId = accId;

            ArchiveAccountCommand archiveAccountCommand = new ArchiveAccountCommand
            {
                ArchiveAccountDto = archiveAccountDto,
            };

            var archiveAccountResponse = await mediator.Send(archiveAccountCommand, cancellationToken).ConfigureAwait(false);

            var endpointResponse = mapper.Map<ArchiveAccountEndpointResponse>(archiveAccountResponse);

            await Send.ResponseAsync(endpointResponse, StatusCodes.Status200OK, cancellationToken).ConfigureAwait(false);
        }
    }
}
