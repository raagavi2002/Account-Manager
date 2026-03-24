// <copyright file="UpdateAccountEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Update
{
    using System.Data;
    using AccountManager.API.Authorization;
    using AccountManager.Application.Commands.UpdateAccountCommand;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Shared.Logging;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// Endpoint for updating account information.
    /// </summary>
    public class UpdateAccountEndpoint(IMediator mediator, IApplogger applogger, AutoMapper.IMapper mapper) : Endpoint<UpdateAccountRequest, UpdateAccountResponse>
    {
        /// <summary>
        /// Configures the update account endpoint.
        /// </summary>
        public override void Configure()
        {
            Put("/accounts/{accountId}");
            AllowAnonymous();
            Version(1);
        }

        /// <summary>
        /// Handles the update account request.
        /// </summary>
        /// <param name="req">The update account request.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public override async Task HandleAsync(UpdateAccountRequest req, CancellationToken ct)
        {
                UpdateAccountDto updateAccountDto = mapper.Map<UpdateAccountDto>(req);

                var command = new UpdateAccountCommand
                {
                    UpdateAccountDto = updateAccountDto,
                };

                var result = await mediator.Send(command, ct).ConfigureAwait(false);

                UpdateAccountResponse response = mapper.Map<UpdateAccountResponse>(result);
                await Send.OkAsync(response, ct).ConfigureAwait(false);
        }
    }
}
