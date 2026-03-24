// <copyright file="ValidateAccountHierarchyEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts
{
    using AccountManager.API.Accounts.Validate_Account_Hierarchy;
    using AccountManager.API.Authorization;
    using AccountManager.Application.Commands.ValidateAccountHierarchyCommand;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// API endpoint for validating account hierarchy relationships.
    /// Accepts a request containing head and sub-account IDs,
    /// sends the validation command via MediatR, and returns the validation result.
    /// </summary>
    /// <param name="mediator">Mediator for sending commands and receiving responses.</param>
    /// <param name="mapper">AutoMapper instance for mapping between DTOs and endpoint responses.</param>
    public class ValidateAccountHierarchyEndpoint : Endpoint<ValidateAccountHierarchyEndpointRequest, ValidateAccountHierarchyEndpointResponse>
    {
        private readonly IMediator mediator;
        private readonly AutoMapper.IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateAccountHierarchyEndpoint"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator used to send commands.</param>
        /// <param name="mapper">The AutoMapper instance used for request/response mapping.</param>
        public ValidateAccountHierarchyEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Configures the endpoint route, version, and summary.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Post("/accounts/validate-hierarchy");
            AllowAnonymous(); // Validated by PreProcessor
            Validator<ValidateAccountHierarchyValidator>();
            Summary(s => new ValidateAccountHierarchyResponse
            {
                IsValid = false,
                ValidationMessages = new List<string>()
                {
                    "Account does not exists",
                },
            });
        }

        /// <summary>
        /// Handles the account hierarchy validation request by sending a command
        /// through MediatR and mapping the result to an API response.
        /// </summary>
        /// <param name="req">The incoming API request containing head and sub-account IDs.</param>
        /// <param name="ct">Token to cancel the operation if needed.</param>
        /// <returns>A <see cref="ValidateAccountHierarchyEndpointResponse"/> containing validation results.</returns>
        public override async Task<ValidateAccountHierarchyEndpointResponse> HandleAsync(ValidateAccountHierarchyEndpointRequest req, CancellationToken ct)
        {
            ValidateAccountHierarchyCommand command = new ValidateAccountHierarchyCommand
            {
                HeadAccountId = req.HeadAccountId,
                SubAccountId = req.SubAccountId,
            };

            // Corrected: send the command, not the raw request
            var response = await mediator.Send(command, ct).ConfigureAwait(false);

            return mapper.Map<ValidateAccountHierarchyEndpointResponse>(response);
        }
    }
}
