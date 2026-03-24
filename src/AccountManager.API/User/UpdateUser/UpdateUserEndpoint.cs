// <copyright file="UpdateUserEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.UpdateUser
{
    using AccountManager.Application.Commands.UpdateUserCommand;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// API endpoint for updating user information.
    /// Handles incoming requests, executes the command, and returns the response.
    /// </summary>
    internal sealed class UpdateUserEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        : Endpoint<UpdateUserRequest, UpdateUserResponse>
    {
        /// <summary>
        /// Configures the endpoint route, version, and metadata.
        /// </summary>
        /// <remarks>
        /// - Sets the API version to 1.<br/>
        /// - Maps the endpoint to <c>PUT /users/{userId}</c>.<br/>
        /// - Allows anonymous access (no authentication required).
        /// </remarks>
        public override void Configure()
        {
            Version(1);
            Put("/users/{userId}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the HTTP PUT request to update user information.
        /// </summary>
        /// <param name="request">The <see cref="UpdateUserRequest"/> containing updated user details.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// On success, returns an <see cref="UpdateUserResponse"/> containing:
        /// <list type="bullet">
        /// <item><description><c>UserId</c> – The unique identifier of the updated user.</description></item>
        /// <item><description><c>Email</c> – The updated email address.</description></item>
        /// <item><description><c>UpdatedAt</c> – The timestamp when the update occurred.</description></item>
        /// </list>
        /// </returns>
        public override async Task HandleAsync(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserCommand
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                RequestorId = Guid.Empty, // TODO: Get from UserContext
            };

            var result = await mediator.Send(command, cancellationToken);

            var response = mapper.Map<UpdateUserResponse>(result);
            await Send.OkAsync(response);
        }
    }
}
