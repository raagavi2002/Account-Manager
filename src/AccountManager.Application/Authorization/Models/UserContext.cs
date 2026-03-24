// <copyright file="UserContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Models
{
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Helpers;

    /// <summary>
    /// Represents the authenticated user's contextual information used for authorization
    /// and permission evaluation within the application layer.
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// Gets the unique identifier of the authenticated user.
        /// </summary>
        required public string UserId { get; init; }

        /// <summary>
        /// Gets the account identifier that represents the current authorization context.
        /// </summary>
        required public string AccountId { get; init; }

        /// <summary>
        /// Gets the set of roles assigned to the user.
        /// </summary>
        required public List<UserRoleType> Roles { get; init; }

        /// <summary>
        /// Gets the list of account identifiers the user is explicitly assigned to.
        /// Used to validate access for internal users such as Account Managers and CSMs.
        /// </summary>
        public List<string> AssignedAccountIds { get; init; } = new ();

        /// <summary>
        /// Gets or sets the current session identifier used for session-scoped caching.
        /// </summary>
        public string? SessionId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user has any internal role
        /// (Admin, Account Manager, or CSM).
        /// </summary>
        public bool IsInternal => Roles.Any(r => UserRoleExtensions.IsInternalUser(r));

        /// <summary>
        /// Gets a value indicating whether the user has any client role
        /// (Main Client, Invoicing Client, or Operations Client).
        /// </summary>
        public bool IsClient => Roles.Any(r => r.IsClientUser());

        /// <summary>
        /// Gets a value indicating whether the user is an administrator.
        /// Administrators have access to all accounts.
        /// </summary>
        public bool IsAdmin => Roles.Contains(UserRoleType.Admin);

        /// <summary>
        /// Validates that internal roles are not combined
        /// Per SBR-ACM-010: "Internal Roles Exclusive: Admin, Account Manager, and CSM cannot be combined"
        /// </summary>
        /// <returns>flag representing whether it has valid role combination.</returns>
        public bool HasValidRoleCombination()
        {
            var internalRoles = Roles.Where(r => r.IsInternalUser()).ToList();

            // A user can have multiple client roles, but only ONE internal role
            return internalRoles.Count <= 1;
        }

        /// <summary>
        /// Checks if user has access to a specific account.
        /// </summary>
        /// <param name="targetAccountId">The account ID to check access for.</param>
        /// <returns>flag representing whether it can access account.</returns>
        public bool CanAccessAccount(string targetAccountId)
        {
            // Admin can access all accounts
            if (IsAdmin)
            {
                return true;
            }

            // Client users can only access their own account
            if (IsClient)
            {
                return AccountId == targetAccountId;
            }

            // Internal users (Account Manager, CSM) can only access assigned accounts
            return AssignedAccountIds.Contains(targetAccountId);
        }
    }
}
