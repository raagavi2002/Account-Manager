// <copyright file="PermissionValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Authorization
{
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Authorization.Models;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Shared.Logging;

    /// <summary>
    /// Validates user permissions and account access by evaluating effective permissions
    /// and enforcing authorization rules.
    /// </summary>
    public class PermissionValidator : IPermissionValidator
    {
        private readonly IPermissionCalculator permissionCalculator;
        private readonly ISessionPermissionCache sessionPermissionCache;
        private readonly IApplogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionValidator"/> class.
        /// </summary>
        /// <param name="permissionCalculator">
        /// Service responsible for computing effective permissions and their sources.
        /// </param>
        /// <param name="logger">
        /// Application logger used for audit and warning logs.
        /// </param>
        public PermissionValidator(
            IPermissionCalculator permissionCalculator,
            ISessionPermissionCache sessionPermissionCache,
            IApplogger logger)
        {
            this.permissionCalculator = permissionCalculator;
            this.sessionPermissionCache = sessionPermissionCache;
            this.logger = logger;
        }

        /// <summary>
        /// Validates whether the specified user has a required permission.
        /// </summary>
        /// <param name="userContext">The current user context.</param>
        /// <param name="requiredPermission">The permission to validate.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        /// A <see cref="PermissionResult"/> indicating whether the permission was granted or denied.
        /// </returns>
        public async Task<PermissionResult> ValidatePermissionAsync(
            UserContext userContext,
            string requiredPermission,
            CancellationToken cancellationToken = default)
        {
            // Get effective permissions
            var effectivePermissions = await sessionPermissionCache
                .GetOrCalculateEffectivePermissionsAsync(userContext, cancellationToken);

            var isGranted = effectivePermissions.Contains(requiredPermission);

            // Determine source
            var source = PermissionSource.None;
            if (isGranted)
            {
                source = permissionCalculator.GetPermissionSourceAsync(
                    userContext,
                    requiredPermission,
                    cancellationToken);
            }

            var result = isGranted
                ? PermissionResult.Granted(requiredPermission, source)
                : PermissionResult.Denied(
                    requiredPermission,
                    $"User does not have permission: {requiredPermission}");

            return result;
        }

        /// <summary>
        /// Validates whether the specified user has access to a target account.
        /// </summary>
        /// <param name="userContext">The current user context.</param>
        /// <param name="targetAccountId">The identifier of the account being accessed.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        /// <c>true</c> if the user has access to the account; otherwise, <c>false</c>.
        /// </returns>
        public bool ValidateAccountAccessAsync(
            UserContext userContext,
            string targetAccountId,
            CancellationToken cancellationToken = default)
        {
            // Use built-in method from UserContext
            var hasAccess = userContext.CanAccessAccount(targetAccountId);

            if (!hasAccess)
            {
                logger.LogError(
                    $"Account access denied for user {userContext.UserId} attempting to access account {userContext.AccountId}",
                    userContext.UserId,
                    targetAccountId);
            }

            return hasAccess;
        }

        /// <summary>
        /// Validates multiple permissions for a user in a single operation.
        /// </summary>
        /// <param name="userContext">The current user context.</param>
        /// <param name="permissions">The collection of permissions to validate.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        /// A dictionary mapping each permission to its corresponding <see cref="PermissionResult"/>.
        /// </returns>
        public async Task<Dictionary<string, PermissionResult>> ValidateMultiplePermissionsAsync(
            UserContext userContext,
            IEnumerable<string> permissions,
            CancellationToken cancellationToken = default)
        {
            var results = new Dictionary<string, PermissionResult>();

            var effectivePermissions = await sessionPermissionCache
                .GetOrCalculateEffectivePermissionsAsync(userContext, cancellationToken);

            foreach (var permission in permissions)
            {
                var isGranted = effectivePermissions.Contains(permission);

                var source = PermissionSource.None;
                if (isGranted)
                {
                    source = permissionCalculator.GetPermissionSourceAsync(
                        userContext,
                        permission,
                        cancellationToken);
                }

                results[permission] = isGranted
                    ? PermissionResult.Granted(permission, source)
                    : PermissionResult.Denied(
                        permission,
                        $"User does not have permission: {permission}");
            }

            return results;
        }

        /// <summary>
        /// Ensures that the specified user has a required permission.
        /// </summary>
        /// <param name="userContext">The current user context.</param>
        /// <param name="requiredPermission">The permission that must be granted.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <exception cref="PermissionDeniedException">
        /// Thrown when the user does not have the required permission.
        /// </exception>
        public async Task EnsurePermissionAsync(
            UserContext userContext,
            string requiredPermission,
            CancellationToken cancellationToken = default)
        {
            var result = await ValidatePermissionAsync(
                userContext,
                requiredPermission,
                cancellationToken);

            if (!result.IsGranted)
            {
                throw new PermissionDeniedException(new Domain.Errors.ErrorResponses
                {
                    Code = "PermissionDenied",
                    Message = $"User does not have required permission: {requiredPermission}",
                    Details = new Domain.Errors.ErrorInfo
                    {
                        AdditionalInfo = new Dictionary<string, string>
                        {
                            { "UserId", userContext.UserId },
                            { "AccountId", userContext.AccountId },
                            { "RequiredPermission", requiredPermission },
                            { "DenialReason", result.DenialReason ?? "No reason provided" },
                        },
                        AccountId = Guid.TryParse(
                            userContext.AccountId,
                            out var accountId)
                            ? accountId
                            : (Guid?)null,
                    },
                });
            }
        }
    }
}
