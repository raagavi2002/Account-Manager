// <copyright file="GetUserEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.GetUser
{
    using AccountManager.Application.Queries.GetUserQuery;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// API endpoint for retrieving user profile information.
    /// Handles incoming requests, executes the query, and returns the response.
    /// </summary>
    internal sealed class GetUserEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        : Endpoint<GetUserRequest, GetUserResponse>
    {
        /// <summary>
        /// Configures the endpoint route, version, and metadata.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Get("/users/{userId}");
            AllowAnonymous(); // Validated by PreProcessor
        }

        /// <summary>
        /// Handles the HTTP GET request to retrieve user profile.
        /// </summary>
        public override async Task HandleAsync(GetUserRequest request, CancellationToken cancellationToken)
        {
            var query = new GetUserQueryRequest
            {
                UserId = request.UserId,
                RequestorId = Guid.Empty, // TODO: Get from UserContext
            };

            var result = await mediator.Send(query, cancellationToken);

            var response = mapper.Map<GetUserResponse>(result);
            await Send.OkAsync(response);
        }
    }
}
