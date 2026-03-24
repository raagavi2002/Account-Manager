// <copyright file="UnlinkSubAccountEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Unlink_Sub_Account
{
    using AccountManager.API.Authorization;
    using AccountManager.Application.Commands.UnlinkSubAccountCommand;
    using AccountManager.Domain.DTO;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// API endpoint for unlinking a sub-account from a head account.
    /// Provides request handling, validation, and response mapping.
    /// </summary>
    internal sealed class UnlinkSubAccountEndpoint
        : Endpoint<UnlinkSubAccountEndpointRequest, UnlinkSubAccountEndpointResponse>
    {
        private readonly IMediator mediator;
        private readonly AutoMapper.IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlinkSubAccountEndpoint"/> class.
        /// </summary>
        /// <param name="mediator">The mediator used to send unlink commands.</param>
        /// <param name="mapper">The AutoMapper instance used for DTO conversions.</param>
        public UnlinkSubAccountEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Configures the endpoint route, version, and summary metadata.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Delete("/accounts/{headAccountId}/relationships/{subaccountId}");
            Validator<UnlinkSubAccountValidator>();
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Unlink a sub-account from a head account";
                s.Description = "Unlinks a specified sub-account from its head account, providing a reason for the unlinking.";
                s.ResponseExamples[200] = new UnlinkSubAccountEndpointResponse
                {
                    SubAccountId = Guid.NewGuid(),
                    FormerHeadAccountId = Guid.NewGuid(),
                    UnlinkedAt = DateTime.UtcNow,
                    UnlinkedBy = Guid.NewGuid(),
                    Reason = "Business restructuring",
                };
            });
        }

        /// <summary>
        /// Handles the unlink request by mapping the request to a DTO,
        /// sending the command via mediator, and returning the appropriate response.
        /// </summary>
        /// <param name="req">The unlink sub-account request payload.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task HandleAsync(UnlinkSubAccountEndpointRequest req, CancellationToken ct)
        {
            UnlinkSubAccountDto unlinkSubAccountDto = mapper.Map<UnlinkSubAccountDto>(req);

            var command = new UnlinkSubAccountCommand
            {
                UnlinkAccountInfo = unlinkSubAccountDto,
            };

            var result = await mediator.Send(command, ct);

            if (result == null || result.SubAccountId == Guid.Empty)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            UnlinkSubAccountEndpointResponse response = mapper.Map<UnlinkSubAccountEndpointResponse>(result);
            await Send.OkAsync(response, ct);
        }
    }
}
