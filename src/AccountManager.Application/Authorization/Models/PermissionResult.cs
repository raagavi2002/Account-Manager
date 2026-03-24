// <copyright file="PermissionResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Models
{
    using AccountManager.Domain.Enums;

    /// <summary>
    /// Represents the outcome of a permission evaluation operation.
    /// </summary>
    /// <remarks>
    /// A permission result indicates whether a specific permission was granted,
    /// the source from which it was granted, and an optional reason when denied.
    /// </remarks>
    public class PermissionResult
    {
        /// <summary>
        /// Gets a value indicating whether the permission was granted.
        /// </summary>
        public bool IsGranted { get; init; }

        /// <summary>
        /// Gets the permission key that was evaluated.
        /// </summary>
        required public string Permission { get; init; }

        /// <summary>
        /// Gets the reason the permission was denied, if applicable.
        /// </summary>
        public string? DenialReason { get; init; }

        /// <summary>
        /// Gets the source from which the permission was granted.
        /// </summary>
        public PermissionSource Source { get; init; }

        /// <summary>
        /// Creates a <see cref="PermissionResult"/> representing a granted permission.
        /// </summary>
        /// <param name="permission">The permission key that was granted.</param>
        /// <param name="source">The source from which the permission was granted.</param>
        /// <returns>A granted <see cref="PermissionResult"/> instance.</returns>
        public static PermissionResult Granted(string permission, PermissionSource source)
        {
            return new PermissionResult
            {
                IsGranted = true,
                Permission = permission,
                Source = source,
            };
        }

        /// <summary>
        /// Creates a <see cref="PermissionResult"/> representing a denied permission.
        /// </summary>
        /// <param name="permission">The permission key that was evaluated.</param>
        /// <param name="reason">The reason the permission was denied.</param>
        /// <returns>A denied <see cref="PermissionResult"/> instance.</returns>
        public static PermissionResult Denied(string permission, string reason)
        {
            return new PermissionResult
            {
                IsGranted = false,
                Permission = permission,
                DenialReason = reason,
                Source = PermissionSource.None,
            };
        }
    }
}
