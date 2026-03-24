// <copyright file="ISessionPermissionCache.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Authorization.Models;

    /// <summary>
    /// Manages caching of computed permissions for the duration of a user session.
    /// </summary>
    public interface ISessionPermissionCache
    {
        /// <summary>
        /// Retrieves effective permissions for the provided session from cache when possible;
        /// otherwise computes them and stores the result.
        /// </summary>
        /// <param name="userContext">User context that contains roles and optional session information.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Effective permission identifiers.</returns>
        Task<HashSet<string>> GetOrCalculateEffectivePermissionsAsync(
            UserContext userContext,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Invalidates cached permissions for a specific session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task InvalidateSessionAsync(string sessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Invalidates all cached session permissions for the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task InvalidateUserAsync(string userId, CancellationToken cancellationToken = default);
    }
}

