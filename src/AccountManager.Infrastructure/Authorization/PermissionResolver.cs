// <copyright file="PermissionResolver.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Authorization
{
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Authorization.Models;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides functionality to resolve and validate user permission contexts.
    /// </summary>
    public class PermissionResolver : IPermissionResolver
    {
        private readonly IUserRepository userRepository;
        private readonly IApplogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionResolver"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for accessing user data and roles.</param>
        /// <param name="logger">Application logger for recording authorization events.</param>
        public PermissionResolver(
            IUserRepository userRepository,
            IApplogger logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Resolves the <see cref="UserContext"/> for a given user and account.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="accountId">Optional account identifier. If not provided, the user's primary account is used.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="UserContext"/> containing user roles, account assignments, and context information.</returns>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the user is not found or inactive.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no account context can be resolved.
        /// </exception>
        public async Task<UserContext> ResolveUserContextAsync(
            string userId,
            string? accountId = null,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Resolving user context for userId: {UserId}, accountId: {AccountId}", userId, accountId ?? "null");

            // Fetch user with roles and account assignments
            var user = await userRepository.GetUserWithRolesAsync(userId, cancellationToken);

            if (user == null)
            {
                logger.LogError("User {UserId} not found", userId);
                throw new UnauthorizedAccessException($"User {userId} not found");
            }

            if (!user.IsActive)
            {
                logger.LogError("Inactive user {UserId} attempted to access system", userId);
                throw new UnauthorizedAccessException($"User {userId} is inactive");
            }

            var contextAccountId = accountId;

            /*if (string.IsNullOrEmpty(contextAccountId))
            {
                logger.LogError("Account context is required but not provided for user {UserId}", userId);
                throw new InvalidOperationException("Account context is required but not provided");
            }*/

            var userContext = new UserContext
            {
                UserId = userId,
                AccountId = contextAccountId,
                Roles = user.Roles
                            .Select(r =>
                            {
                                return EnumParser.TryParse<UserRoleType>(r, out var parsed)
                                    ? parsed
                                    : default; // or handle invalid case differently
                            })
                            .ToList(),

                AssignedAccountIds = new List<string>() { contextAccountId },
            };

            logger.LogInformation(
                "Resolved user context for {UserId}: Roles={Roles}, AccountId={AccountId}, AssignedAccounts={Count}",
                userId,
                string.Join(", ", user.Roles.Select(r => r.ToString())),
                contextAccountId);

            return userContext;
        }

        /// <summary>
        /// Validates the provided <see cref="UserContext"/> against role and account access rules.
        /// </summary>
        /// <param name="userContext">The user context to validate.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns><c>true</c> if the context is valid; otherwise, <c>false</c>.</returns>
        public bool ValidateContextAsync(UserContext userContext, CancellationToken cancellationToken = default)
        {
            // Validate role combination (per SBR-ACM-010)
            if (!userContext.HasValidRoleCombination())
            {
                logger.LogError(
                    "User {UserId} has invalid role combination. Internal roles cannot be combined. Roles: {Roles}",
                    userContext.UserId,
                    string.Join(", ", userContext.Roles));
                return false;
            }

            // Validate account access
            if (!userContext.CanAccessAccount(userContext.AccountId))
            {
                logger.LogError(
                    $"User {userContext.UserId} cannot access account {userContext.AccountId}. IsInternal={userContext.IsInternal}, IsClient={userContext.IsClient}, AssignedAccounts={string.Join(", ", userContext.AssignedAccountIds)}");
                return false;
            }

            logger.LogInformation("User context validated successfully for {UserId}", userContext.UserId);
            return true;
        }
    }
}
