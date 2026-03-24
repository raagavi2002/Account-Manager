// <copyright file="IPermissionCalculator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Authorization.Models;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Results;

    /// <summary>
    /// Provides functionality to calculate and evaluate effective permissions for a user
    /// by combining role-based permissions with user-specific overrides.
    /// </summary>
    public interface IPermissionCalculator
    {
        /// <summary>
        /// Computes all effective permissions for a user within a given account context.
        /// </summary>
        /// <param name="userContext">
        /// The context containing user, account, role, and override information.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A set of permission identifiers representing the user's effective permissions.
        /// </returns>
        HashSet<string> ComputeEffectivePermissionsAsync(
            UserContext userContext,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the base permissions associated with a specific role.
        /// </summary>
        /// <param name="role">
        /// The role for which to retrieve base permissions.
        /// </param>
        /// <returns>
        /// A set of permission identifiers granted by the specified role.
        /// </returns>
        HashSet<string> GetRolePermissions(UserRoleType role);

        /// <summary>
        /// Determines whether a specific permission is granted to the user.
        /// </summary>
        /// <param name="userContext">
        /// The context containing user, account, role, and override information.
        /// </param>
        /// <param name="permission">
        /// The permission identifier to check.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// <c>true</c> if the permission is granted; otherwise, <c>false</c>.
        /// </returns>
        bool HasPermissionAsync(
            UserContext userContext,
            string permission,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the source from which a specific permission is granted.
        /// </summary>
        /// <param name="userContext">
        /// The context containing user, account, role, and override information.
        /// </param>
        /// <param name="permission">
        /// The permission identifier to evaluate.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A <see cref="PermissionSource"/> value indicating whether the permission
        /// originates from a role, a user override, or both.
        /// </returns>
        PermissionSource GetPermissionSourceAsync(
            UserContext userContext,
            string permission,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Validates whether the supplied <paramref name="roles"/> form a legal combination
        /// under business rule EBR-ACM-004.
        /// </summary>
        /// <param name="roles">
        /// The role values to validate (IN-01). Must not be <c>null</c>.
        /// </param>
        /// <param name="userId">
        /// Optional user identifier for contextual messages during update scenarios (IN-02).
        /// Has no effect on rule enforcement.
        /// </param>
        /// <returns>
        /// A <see cref="RoleValidationResult"/> with <see cref="RoleValidationResult.IsValid"/>,
        /// <see cref="RoleValidationResult.ValidationMessages"/>, and
        /// <see cref="RoleValidationResult.AllowedCombinations"/> populated.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="roles"/> is <c>null</c>.
        /// </exception>
        RoleValidationResult Validate(List<UserRoleType> roles, Guid? userId = null);
    }
}
