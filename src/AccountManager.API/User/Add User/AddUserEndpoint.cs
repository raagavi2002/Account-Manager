/// <copyright file="AddUserEndpoint.cs" company="PlaceholderCompany">
/// Copyright (c) PlaceholderCompany. All rights reserved.
/// </copyright>

namespace AccountManager.API.User.Add_User
{
    using AccountManager.API.Authorization;
    using AccountManager.Application.Commands.AddUserCommand;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AutoMapper;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// API endpoint for adding a new user to an account.
    /// Handles incoming requests, maps them to domain DTOs,
    /// executes the <see cref="AddUserCommand"/>, and returns the response.
    /// </summary>
    internal sealed class AddUserEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        : Endpoint<AddUserEndpointRequest, AddUserEndpointResponse>
    {
        /// <summary>
        /// Configures the endpoint route, version, and metadata.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Post("/accounts/{accountId}/users");
            AllowAnonymous(); // Validated by PreProcessor
            Validator<AddUserValidator>();
            Summary(s =>
            {
                s.Summary = "Add a new user to an account";
                s.Description = "Adds a new user to the specified account.";
                s.ResponseExamples[201] = new AddUserEndpointResponse
                {
                    UserId = Guid.NewGuid(),
                    Email = "example@email.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Roles = new List<UserRoleType> { UserRoleType.Admin },
                };
            });
        }

        /// <summary>
        /// Handles the incoming request to add a user.
        /// Maps the request to a DTO, sends the command via MediatR,
        /// and returns the created user information in the response.
        /// </summary>
        /// <param name="req">The request containing user details.</param>
        /// <param name="ct">The cancellation token.</param>
        public override async Task HandleAsync(AddUserEndpointRequest req, CancellationToken ct)
        {
            req.AccountId = Route<Guid>("accountId");
            AddUserDto addUserDto = mapper.Map<AddUserDto>(req);
            var command = new AddUserCommand
            {
                AddUser = addUserDto,
            };

            var addUserInfo = await mediator.Send(command, ct).ConfigureAwait(false);
            var addUserResponse = mapper.Map<AddUserEndpointResponse>(addUserInfo);
            await Send.ResponseAsync(addUserResponse, StatusCodes.Status201Created, ct).ConfigureAwait(false);
        }
    }
}
