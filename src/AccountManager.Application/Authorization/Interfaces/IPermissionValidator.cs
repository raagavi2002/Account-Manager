// <copyright file="IPermissionValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Authorization.Models;

    /// <summary>
    /// Provides methods to validate user permissions and make authorization decisions
    /// for protected actions and resources.
    /// </summary>
    public interface IPermissionValidator
    {
        /// <summary>
        /// Validates whether the user has the required permission.
        /// </summary>
        /// <param name="userContext">
        /// The context containing user, account, role, and override information.
        /// </param>
        /// <param name="requiredPermission">
        /// The permission identifier required to perform the action.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A <see cref="PermissionResult"/> describing whether the permission was granted
        /// and any relevant authorization details.
        /// </returns>
        /// <remarks>
        /// Implementations are expected to log the authorization decision
        /// to the audit log.
        /// </remarks>
        Task<PermissionResult> ValidatePermissionAsync(
            UserContext userContext,
            string requiredPermission,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates whether the user is authorized to access a specific account.
        /// </summary>
        /// <param name="userContext">
        /// The context containing user and account-related information.
        /// </param>
        /// <param name="targetAccountId">
        /// The identifier of the account being accessed.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// <c>true</c> if the user is authorized to access the account;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This typically includes validation of account assignments for internal users.
        /// </remarks>
        bool ValidateAccountAccessAsync(
            UserContext userContext,
            string targetAccountId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates multiple permissions in a single operation.
        /// </summary>
        /// <param name="userContext">
        /// The context containing user, account, role, and override information.
        /// </param>
        /// <param name="permissions">
        /// A collection of permission identifiers to validate.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A dictionary mapping each permission identifier to its corresponding
        /// <see cref="PermissionResult"/>.
        /// </returns>
        /// <remarks>
        /// Useful for complex operations that require multiple permissions to be evaluated
        /// together.
        /// </remarks>
        Task<Dictionary<string, PermissionResult>> ValidateMultiplePermissionsAsync(
            UserContext userContext,
            IEnumerable<string> permissions,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Ensures that the user has the required permission.
        /// </summary>
        /// <param name="userContext">
        /// The context containing user, account, role, and override information.
        /// </param>
        /// <param name="requiredPermission">
        /// The permission identifier required to perform the action.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A task that completes when the permission check has finished.
        /// </returns>
        /// <remarks>
        /// Implementations should throw an authorization-related exception
        /// if the permission is denied. Use this method when failing fast is desired.
        /// </remarks>
        Task EnsurePermissionAsync(
            UserContext userContext,
            string requiredPermission,
            CancellationToken cancellationToken = default);
    }
}
