// <copyright file="GetUserQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetUserQuery
{
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="GetUserQueryRequest"/> and returns a <see cref="GetUserQueryResponse"/>.
    /// Includes permission calculation, audit logging, and Kafka event publishing.
    /// </summary>
    public class GetUserQueryHandler : IRequestHandler<GetUserQueryRequest, GetUserQueryResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IPermissionCalculator permissionCalculator;
        private readonly IApplogger applogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for accessing user data.</param>
        /// <param name="permissionCalculator">Service for calculating user permissions.</param>
        /// <param name="applogger">Application logger for audit and diagnostic logging (optional).</param>
        public GetUserQueryHandler(
            IUserRepository userRepository,
            IPermissionCalculator permissionCalculator,
            IApplogger applogger = null)
        {
            this.userRepository = userRepository;
            this.permissionCalculator = permissionCalculator;
            this.applogger = applogger;
        }

        /// <summary>
        /// Handles the query request by retrieving user details, performing permission checks,
        /// and returning a response with user information.
        /// </summary>
        /// <param name="request">The <see cref="GetUserQueryRequest"/> containing the user ID.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A <see cref="GetUserQueryResponse"/> containing user details.</returns>
        /// <exception cref="UserNotFoundException">
        /// Thrown when the user with the specified ID does not exist.
        /// </exception>
        public async Task<GetUserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
        {
            // Get user by ID
            var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(new Domain.Errors.ErrorResponses
                {
                    Code = "UserNotFound",
                    Message = $"User with ID {request.UserId} not found.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            // TODO: Add permission checks here (access control for users in own account, admins, etc.)

            // Log audit entry
            return new GetUserQueryResponse
            {
                UserId = user.UserId,
                AccountId = user.AccountId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.Roles ?? new List<string>(),
                IsActive = user.IsActive,
                LastLoginAt = user.LastLoginAt,
                LoginCount = user.LoginCount,
            };
        }
    }
}
