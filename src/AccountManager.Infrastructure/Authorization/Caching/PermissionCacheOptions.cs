// <copyright file="PermissionCacheOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Authorization.Caching
{
    /// <summary>
    /// Configuration options for session-based permission caching.
    /// </summary>
    public class PermissionCacheOptions
    {
        /// <summary>
        /// Gets or sets the time-to-live, in minutes, for cached session permission entries.
        /// Defaults to 30 minutes.
        /// </summary>
        public int SessionTtlMinutes { get; set; } = 30;

        /// <summary>
        /// Gets or sets a cache key prefix to avoid collisions across environments/services.
        /// </summary>
        public string KeyPrefix { get; set; } = "permissions";
    }
}

