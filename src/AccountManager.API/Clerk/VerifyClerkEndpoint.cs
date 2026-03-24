// <copyright file="VerifyClerkEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Clerk
{
    using AccountManager.Application.Abstractions;
    using FastEndpoints;

    /// <summary>
    /// API endpoint for verifying Clerk API connectivity.
    /// </summary>
    internal sealed class VerifyClerkEndpoint(IClerkService clerkService)
        : EndpointWithoutRequest<bool>
    {
        /// <summary>
        /// Configures the endpoint route, version, and metadata.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Get("/clerk/verify");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Verify Clerk API connectivity";
                s.Description = "Checks if the Clerk API is accessible and working.";
            });
        }

        /// <summary>
        /// Handles the request to verify Clerk API.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            var isWorking = await clerkService.VerifyApiAsync(cancellationToken);
            await SendAsync(isWorking, cancellationToken: cancellationToken);
        }
    }
}