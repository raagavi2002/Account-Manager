// <copyright file="CreateAccountEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Create
{
    using AccountManager.API.Accounts.GetAccountDetails;
    using AccountManager.API.Authorization;
    using AccountManager.Application.Commands.CreateAccountCommand;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Exceptions;
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Endpoint for creating a new client account.
    /// </summary>
    internal sealed class CreateAccountEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        : Endpoint<CreateAccountRequest, CreateAccountResponse>
    {
        /// <summary>
        /// Configures the endpoint for creating a new account.
        /// </summary>
        public override void Configure()
        {
            Post("/api/v1/account-manager/accounts");
            Version(1);
            AllowAnonymous();
            PreProcessor<PermissionPreProcessor<CreateAccountRequest>>();
            Validator<CreateAccountValidator>();

            Summary(s =>
            {
                s.Summary = "Create a new client account";
                s.Description = "Creates a new client account with the provided details. Requires Admin or AccountManager role.";
                s.ResponseExamples[201] = new CreateAccountResponse
                {
                    AccountId = Guid.NewGuid(),
                    AccountName = "Example Corp",
                    AccountType = "PROFESSIONAL",
                    Currency = "USD",
                    Timezone = "Asia/Kolkata",
                    AccountStatus = "Inactive",
                    Version = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
            });
        }

        /// <summary>
        /// Handles the creation of a new account.
        /// </summary>
        /// <param name="req">The request containing account details.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task HandleAsync(CreateAccountRequest req, CancellationToken ct)
        {
            // Map request → DTO
            CreateAccountDto dto = mapper.Map<CreateAccountDto>(req);

            var command = new CreateAccountCommand
            {
                Account = dto,
            };

            var result = await mediator.Send(command, ct).ConfigureAwait(false);

            // Map command response → API response
            var response = mapper.Map<CreateAccountResponse>(result);

            // Return 201 Created with the response
            await Send.ResponseAsync(response, StatusCodes.Status201Created, ct).ConfigureAwait(false);
        }
    }
}
