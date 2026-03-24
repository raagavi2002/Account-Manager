// <copyright file="SessionPermissionCache.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Authorization.Caching
{
    using System.Text.Json;
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Authorization.Models;
    using AccountManager.Shared.Logging;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Redis-backed implementation of <see cref="ISessionPermissionCache"/> using <see cref="IDistributedCache"/>.
    /// </summary>
    public class SessionPermissionCache : ISessionPermissionCache
    {
        private readonly IDistributedCache distributedCache;
        private readonly IPermissionCalculator permissionCalculator;
        private readonly IOptions<PermissionCacheOptions> options;
        private readonly IApplogger logger;

        private readonly JsonSerializerOptions jsonOptions = new (JsonSerializerDefaults.Web);

        public SessionPermissionCache(
            IDistributedCache distributedCache,
            IPermissionCalculator permissionCalculator,
            IOptions<PermissionCacheOptions> options,
            IApplogger logger)
        {
            this.distributedCache = distributedCache;
            this.permissionCalculator = permissionCalculator;
            this.options = options;
            this.logger = logger;
        }

        public async Task<HashSet<string>> GetOrCalculateEffectivePermissionsAsync(
            UserContext userContext,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(userContext);

            if (string.IsNullOrWhiteSpace(userContext.SessionId))
            {
                logger.LogInformation(
                    $"SessionId missing for user '{userContext.UserId}'; computing permissions without session cache.",
                    new { userContext.UserId });

                return permissionCalculator.ComputeEffectivePermissionsAsync(userContext, cancellationToken);
            }

            var cacheKey = BuildSessionKey(userContext.SessionId);

            try
            {
                var cachedJson = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
                if (!string.IsNullOrWhiteSpace(cachedJson))
                {
                    var entry = JsonSerializer.Deserialize<PermissionCacheEntry>(cachedJson, jsonOptions);

                    if (entry is not null && entry.Permissions is not null && IsValidForContext(entry, userContext))
                    {
                        return entry.Permissions.ToHashSet(StringComparer.Ordinal);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogException(
                    ex,
                    $"Failed to read permissions cache for session '{userContext.SessionId}'; falling back to calculation.",
                    new { userContext.SessionId });
            }

            var calculated = permissionCalculator.ComputeEffectivePermissionsAsync(userContext, cancellationToken);

            await TryCacheAsync(userContext, calculated, cancellationToken);

            return calculated;
        }

        public async Task InvalidateSessionAsync(string sessionId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return;
            }

            var sessionKey = BuildSessionKey(sessionId);

            try
            {
                await distributedCache.RemoveAsync(sessionKey, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogException(ex, $"Failed to invalidate permissions cache for session '{sessionId}'", new { sessionId });
            }
        }

        public async Task InvalidateUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var indexKey = BuildUserIndexKey(userId);
            string? cachedIndex;

            try
            {
                cachedIndex = await distributedCache.GetStringAsync(indexKey, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogException(ex, $"Failed to read permissions cache index for user '{userId}'", new { userId });
                return;
            }

            if (string.IsNullOrWhiteSpace(cachedIndex))
            {
                return;
            }

            HashSet<string>? sessionIds;
            try
            {
                sessionIds = JsonSerializer.Deserialize<HashSet<string>>(cachedIndex, jsonOptions);
            }
            catch (Exception ex)
            {
                logger.LogException(ex, $"Failed to deserialize permissions cache index for user '{userId}'", new { userId });
                return;
            }

            if (sessionIds is null || sessionIds.Count == 0)
            {
                return;
            }

            foreach (var sessionId in sessionIds)
            {
                await InvalidateSessionAsync(sessionId, cancellationToken);
            }

            try
            {
                await distributedCache.RemoveAsync(indexKey, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogException(ex, $"Failed to remove permissions cache index for user '{userId}'", new { userId });
            }
        }

        private async Task TryCacheAsync(
            UserContext userContext,
            HashSet<string> permissions,
            CancellationToken cancellationToken)
        {
            var sessionId = userContext.SessionId;
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return;
            }

            var ttlMinutes = Math.Max(1, options.Value.SessionTtlMinutes);
            var cacheEntry = new PermissionCacheEntry
            {
                UserId = userContext.UserId,
                Roles = userContext.Roles.Select(r => r.ToString()).ToList(),
                Permissions = permissions.ToList(),
            };

            var cacheKey = BuildSessionKey(sessionId);

            try
            {
                var json = JsonSerializer.Serialize(cacheEntry, jsonOptions);
                await distributedCache.SetStringAsync(
                    cacheKey,
                    json,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ttlMinutes),
                    },
                    cancellationToken);

                await TryUpdateUserIndexAsync(userContext.UserId, sessionId, ttlMinutes, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogException(ex, $"Failed to cache permissions for session '{sessionId}'", new { sessionId });
            }
        }

        private async Task TryUpdateUserIndexAsync(
            string userId,
            string sessionId,
            int ttlMinutes,
            CancellationToken cancellationToken)
        {
            var indexKey = BuildUserIndexKey(userId);

            HashSet<string> sessionIds = new (StringComparer.OrdinalIgnoreCase);

            try
            {
                var existing = await distributedCache.GetStringAsync(indexKey, cancellationToken);
                if (!string.IsNullOrWhiteSpace(existing))
                {
                    var parsed = JsonSerializer.Deserialize<HashSet<string>>(existing, jsonOptions);
                    if (parsed is not null)
                    {
                        sessionIds = new HashSet<string>(parsed, StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogException(ex, $"Failed to read permissions cache index for user '{userId}'", new { userId });
            }

            sessionIds.Add(sessionId);

            try
            {
                var json = JsonSerializer.Serialize(sessionIds, jsonOptions);
                await distributedCache.SetStringAsync(
                    indexKey,
                    json,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ttlMinutes),
                    },
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogException(ex, $"Failed to update permissions cache index for user '{userId}'", new { userId });
            }
        }

        private string BuildSessionKey(string sessionId) => $"{options.Value.KeyPrefix}:session:{sessionId}";

        private string BuildUserIndexKey(string userId) => $"{options.Value.KeyPrefix}:user:{userId}:sessions";

        private static bool IsValidForContext(PermissionCacheEntry entry, UserContext userContext)
        {
            if (!string.Equals(entry.UserId, userContext.UserId, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var cachedRoles = (entry.Roles ?? new List<string>()).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var currentRoles = userContext.Roles.Select(r => r.ToString()).ToHashSet(StringComparer.OrdinalIgnoreCase);

            return cachedRoles.SetEquals(currentRoles);
        }

        private sealed class PermissionCacheEntry
        {
            public string UserId { get; init; } = string.Empty;

            public List<string>? Roles { get; init; }

            public List<string>? Permissions { get; init; }
        }
    }
}
