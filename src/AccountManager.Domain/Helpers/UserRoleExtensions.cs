// <copyright file="UserRoleExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Helpers
{
    using AccountManager.Domain.Enums;

    /// <summary>
    /// Extension methods for <see cref="UserRoleType"/>.
    /// </summary>
    public static class UserRoleExtensions
    {
        /// <summary>
        /// Determines whether the specified user role represents an internal user.
        /// </summary>
        /// <param name="role">The user role to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the role is an internal role (Admin, Account Manager, or CSM);
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInternalUser(this UserRoleType role)
        {
            return role is UserRoleType.Admin
                or UserRoleType.AccountManager
                or UserRoleType.CSM;
        }

        /// <summary>
        /// Determines whether the specified user role represents a client user.
        /// </summary>
        /// <param name="role">The user role to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the role is a client role (Main Client, Invoicing Client, or Operations Client);
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsClientUser(this UserRoleType role)
        {
            return role is UserRoleType.MainClient
                or UserRoleType.InvoicingClient
                or UserRoleType.OperationsClient;
        }

        /// <summary>
        /// Gets a human-readable display name for the specified user role.
        /// </summary>
        /// <param name="role">The user role.</param>
        /// <returns>
        /// A user-friendly display name corresponding to the role.
        /// If the role is not explicitly handled, its enum name is returned.
        /// </returns>
        public static string GetDisplayName(this UserRoleType role)
        {
            return role switch
            {
                UserRoleType.Admin => "Administrator",
                UserRoleType.AccountManager => "Account Manager",
                UserRoleType.CSM => "Customer Success Manager",
                UserRoleType.MainClient => "Main Client",
                UserRoleType.InvoicingClient => "Invoicing Client",
                UserRoleType.OperationsClient => "Operations Client",
                _ => role.ToString()
            };
        }

        /// <summary>
        /// Gets a descriptive explanation of the specified user role and its permissions.
        /// </summary>
        /// <param name="role">The user role.</param>
        /// <returns>
        /// A description of the role’s responsibilities and access level.
        /// Returns an empty string if the role is not explicitly handled.
        /// </returns>
        public static string GetDescription(this UserRoleType role)
        {
            return role switch
            {
                UserRoleType.Admin => "Full system access across all accounts",
                UserRoleType.AccountManager => "Manages assigned accounts with full control",
                UserRoleType.CSM => "Customer support for assigned accounts with limited updates",
                UserRoleType.MainClient => "Primary client contact with read access to all account data",
                UserRoleType.InvoicingClient => "Client user focused on financial and billing data",
                UserRoleType.OperationsClient => "Client user with operational data access only",
                _ => string.Empty
            };
        }

    }

}
