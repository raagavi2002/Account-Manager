// <copyright file="GetTimezoneEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetTimezone
{
    using AccountManager.API.Accounts.GetAccountDetails;
    using AccountManager.Application.Queries.GetTimezoneQuery;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// Endpoint for retrieving a list of supported timezones.
    /// </summary>
    public class GetTimezoneEndpoint : EndpointWithoutRequest<GetTimezoneResponse>
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTimezoneEndpoint"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used to send queries.</param>
        public GetTimezoneEndpoint(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <inheritdoc/>
        public override void Configure()
        {
            Version(1);
            Get("/timezones");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Get Supported Timezones";
                s.Description = "Retrieves a list of supported timezones.";
                s.Response<GetTimezoneResponse>(StatusCodes.Status200OK, "Successfully retrieved the list of supported timezones.");
            });
        }

        /// <summary>
        /// Handles the request to retrieve a list of supported timezones.
        /// </summary>
        /// <param name="req">The empty request object.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task HandleAsync(CancellationToken ct)
        {
            var command = new GetTimezoneQueryRequest();
            var response = await mediator.Send(command).ConfigureAwait(false);
            var apiResponse = new GetTimezoneResponse
            {
                Timezones = response.Timezones,
            };
            await Send.OkAsync(apiResponse, ct);
        }
    }
}
