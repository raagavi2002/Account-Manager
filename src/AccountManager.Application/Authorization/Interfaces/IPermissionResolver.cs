// <copyright file="IPermissionResolver.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Authorization.Models;

    /// <summary>
    /// Resolves user-related authorization data from authentication information
    /// and builds a complete permission context for downstream authorization checks.
    /// </summary>
    public interface IPermissionResolver
    {
        /// <summary>
        /// Resolves and builds a <see cref="UserContext"/> for an authenticated user.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the authenticated user.
        /// </param>
        /// <param name="accountId">
        /// The optional account identifier to scope the user context.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A fully populated <see cref="UserContext"/> containing roles,
        /// account assignments, and permission-related data.
        /// </returns>
        /// <remarks>
        /// Implementations typically fetch role assignments, account mappings,
        /// and user-specific overrides from persistent storage.
        /// </remarks>
        Task<UserContext> ResolveUserContextAsync(
            string userId,
            string? accountId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates that the resolved user context is valid for the requested operation.
        /// </summary>
        /// <param name="userContext">
        /// The user context to validate.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// <c>true</c> if the user context is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Validation may include:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Ensuring invalid role combinations are not present
        /// (for example, internal roles cannot be combined).
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Verifying account access rules, such as clients accessing only their own
        /// accounts and internal users accessing only assigned accounts.
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        bool ValidateContextAsync(
            UserContext userContext,
            CancellationToken cancellationToken = default);
    }
}
