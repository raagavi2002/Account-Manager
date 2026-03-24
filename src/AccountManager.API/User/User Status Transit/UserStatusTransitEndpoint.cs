// <copyright file="UserStatusTransitEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.User_Status_Transit
{
    using AccountManager.API.Authorization;
    using AccountManager.Application.Commands.UserStatusTransitCommand;
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Endpoint for changing user activation state between ACTIVE and INACTIVE.
    /// </summary>
    internal sealed class UserStatusTransitEndpoint(
        IMediator mediator,
        AutoMapper.IMapper mapper)
        : Endpoint<UserStatusTransitEndpointRequest, UserStatusTransitEndpointResponse>
    {
        /// <summary>
        /// Configures the endpoint.
        /// </summary>
        public override void Configure()
        {
            Put("/users/{userId}/status");
            Version(1);
            AllowAnonymous(); // Validated by PreProcessor


            Summary(s =>
            {
                s.Summary = "Update user activation state";
                s.Description = "Changes user activation state between ACTIVE and INACTIVE. Consolidates activate/deactivate operations into a single endpoint.";

                s.ResponseExamples[200] = new UserStatusTransitEndpointResponse
                {
                    UserId = Guid.NewGuid(),
                    IsActive = true,
                    StatusChangedAt = DateTime.UtcNow,
                    StatusChangedBy = Guid.NewGuid(),
                    Version = 1,
                    Reason = "Reactivating user after successful appeal.",
                };
            });
        }

        /// <summary>
        /// Handles the user status transition.
        /// </summary>
        /// <param name="req">Request model.</param>
        /// <param name="ct">Cancellation token.</param>
        public override async Task HandleAsync(UserStatusTransitEndpointRequest req, CancellationToken ct)
        {
            var command = new UserStatusTransitCommand
            {
                UserId = req.UserId,
                TargetStatus = req.TargetStatus,
                Reason = req.Reason,
                Version = req.Version,
            };

            var result = await mediator.Send(command, ct).ConfigureAwait(false);

            // Map result → response
            var response = mapper.Map<UserStatusTransitEndpointResponse>(result);

            await Send.ResponseAsync(response, StatusCodes.Status200OK, ct)
                      .ConfigureAwait(false);
        }
    }
}
