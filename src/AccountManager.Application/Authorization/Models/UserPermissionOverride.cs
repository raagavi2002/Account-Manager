// <copyright file="UserPermissionOverride.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Models
{
    /// <summary>
    /// Represents a user-specific permission override applied within an account context.
    /// </summary>
    public class UserPermissionOverride
    {
        /// <summary>
        /// Gets the unique identifier of the user to whom the permission override applies.
        /// </summary>
        required public string UserId { get; init; }

        /// <summary>
        /// Gets the account identifier for which the permission override is granted.
        /// </summary>
        required public string AccountId { get; init; }

        /// <summary>
        /// Gets the permission key that is being granted to the user.
        /// </summary>
        required public string Permission { get; init; }

        /// <summary>
        /// Gets the date and time when the permission override was granted.
        /// </summary>
        public DateTime GrantedAt { get; init; }

        /// <summary>
        /// Gets the identifier of the user or system that granted the permission override.
        /// </summary>
        required public string GrantedBy { get; init; }

        /// <summary>
        /// Gets an optional reason or justification for granting the permission override.
        /// </summary>
        public string? Reason { get; init; }
    }
}
