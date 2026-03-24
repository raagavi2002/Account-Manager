// <copyright file="PermissionCalculator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Authorization.Models;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Enums.Authorization;
    using AccountManager.Domain.Results;
    using AccountManager.Shared.Logging;

    /// <summary>
    /// Calculates effective permissions for a user by combining role-based permissions
    /// and user-specific permission overrides.
    /// </summary>
    /// <remarks>
    /// This implementation currently derives permissions from user roles only.
    /// Support for user-specific overrides can be enabled by wiring the
    /// permission override repository.
    /// </remarks>
    public class PermissionCalculator : IPermissionCalculator
    {
        /// <summary>
        /// Internal roles. Mutually exclusive with <see cref="ClientRoles"/>.
        /// <see cref="UserRoleType.Admin"/> is additionally exclusive with every other role.
        /// </summary>
        private static readonly IReadOnlySet<UserRoleType> InternalRoles = new HashSet<UserRoleType>
        {
            UserRoleType.Admin,
            UserRoleType.AccountManager,
            UserRoleType.CSM,
        };

        /// <summary>
        /// Client roles. Mutually exclusive with <see cref="InternalRoles"/>.
        /// <see cref="UserRoleType.MainClient"/> is additionally exclusive with other client roles.
        /// </summary>
        private static readonly IReadOnlySet<UserRoleType> ClientRoles = new HashSet<UserRoleType>
        {
            UserRoleType.MainClient,
            UserRoleType.InvoicingClient,
            UserRoleType.OperationsClient,
        };

        /// <summary>
        /// Concrete valid combinations surfaced to the caller when validation fails (OUT-03).
        /// </summary>
        private static readonly List<string> SuggestedCombinations =
        [
            "Admin — must be assigned alone",
            "AccountManager — standalone or combined with CSM",
            "CSM — standalone or combined with AccountManager",
            "MainClient — must be assigned alone among client roles",
            "InvoicingClient + OperationsClient",
            "InvoicingClient — standalone",
            "OperationsClient — standalone"
        ];

        private readonly IApplogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionCalculator"/> class.
        /// </summary>
        /// <param name="logger">
        /// The application logger used to log permission calculation details.
        /// </param>
        public PermissionCalculator(IApplogger logger)
        {
            this.logger = logger;
        }

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
        public RoleValidationResult Validate(List<UserRoleType> roles, Guid? userId = null)
        {
            ArgumentNullException.ThrowIfNull(roles);

            var context = userId.HasValue ? $"User '{userId}'" : "User";

            var invalidValues = roles
                .Where(r => !Enum.IsDefined(typeof(UserRoleType), r))
                .ToList();

            if (invalidValues.Count > 0)
            {
                return Fail(new Dictionary<string, string>
                {
                    { "Invalid Role", $"The following role values are invalid: {string.Join(", ", invalidValues)}. Valid roles are: {string.Join(", ", Enum.GetValues<UserRoleType>())}."},
                });
            }

            var uniqueRoles = roles.Distinct().ToHashSet();
            var violations = new Dictionary<string, string>();

            var hasInternal = uniqueRoles.Any(r => InternalRoles.Contains(r));
            var hasClient = uniqueRoles.Any(r => ClientRoles.Contains(r));
            var hasAdmin = uniqueRoles.Contains(UserRoleType.Admin);
            var hasMainClient = uniqueRoles.Contains(UserRoleType.MainClient);

            if (hasAdmin && uniqueRoles.Count > 1)
            {
                violations.Add($"{context}", "'Admin' is an exclusive role and cannot be combined with: " + $"{string.Join(", ", uniqueRoles.Where(r => r != UserRoleType.Admin))}.");
            }

            if (hasMainClient && uniqueRoles.Any(r => ClientRoles.Contains(r) && r != UserRoleType.MainClient))
            {
                violations.Add("Role Validation", "'MainClient' is an exclusive client role and cannot be combined with: " + $"{string.Join(", ", uniqueRoles.Where(r => ClientRoles.Contains(r) && r != UserRoleType.MainClient))}.");
            }

            if (hasInternal && hasClient)
            {
                violations.Add("Invalid Role Combination", $"Internal roles ({string.Join(", ", uniqueRoles.Where(r => InternalRoles.Contains(r)))}) " + $"cannot be combined with client roles ({string.Join(", ", uniqueRoles.Where(r => ClientRoles.Contains(r)))}).");
            }

            return violations.Count > 0
                ? new RoleValidationResult
                {
                    IsValid = false,
                    ValidationMessages = violations,
                    AllowedCombinations = SuggestedCombinations,
                }
                : new RoleValidationResult { IsValid = true };
        }

        /// <inheritdoc />
        public HashSet<string> ComputeEffectivePermissionsAsync(
            UserContext userContext,
            CancellationToken cancellationToken = default)
        {
            var effectivePermissions = new HashSet<string>();

            // Step 1: Load role-based permissions for all user roles
            foreach (var role in userContext.Roles)
            {
                var rolePermissions = GetRolePermissions(role);
                effectivePermissions.UnionWith(rolePermissions);

                logger.LogInformation(
                    "Added {Count} permissions from role {Role} for user {UserId}",
                    rolePermissions.Count,
                    role.ToString(),
                    userContext.UserId);
            }

            /*
            // Step 2: Load and merge user-specific permission overrides
            var overrides = await _overrideRepository.GetUserOverridesAsync(
                userContext.UserId,
                userContext.AccountId,
                cancellationToken);

            var overrideCount = 0;
            foreach (var @override in overrides)
            {
                if (effectivePermissions.Add(@override.Permission))
                {
                    overrideCount++;
                }
            }

            if (overrideCount > 0)
            {
                _logger.LogInformation(
                    "Added {OverrideCount} override permissions for user {UserId} in account {AccountId}",
                    overrideCount,
                    userContext.UserId,
                    userContext.AccountId);
            }
            */

            logger.LogInformation(
                "Computed {PermissionCount} total effective permissions for user {UserId} in account {AccountId}",
                effectivePermissions.Count,
                userContext.UserId,
                userContext.AccountId);

            return effectivePermissions;
        }

        /// <inheritdoc />
        public async Task<HashSet<string>> ComputeEffectivePermissionsAsync(string role)
        {
            var effectivePermissions = new HashSet<string>();
            UserRoleType userRole = Enum.Parse<UserRoleType>(role);
            var rolePermissions = GetRolePermissions(userRole);
            effectivePermissions.UnionWith(rolePermissions);
            logger.LogInformation(
                "Added {Count} permissions from role {Role} for user {UserId}",
                rolePermissions.Count,
                role.ToString());
            return effectivePermissions;
        }

        /// <inheritdoc />
        public bool HasPermissionAsync(
            UserContext userContext,
            string permission,
            CancellationToken cancellationToken = default)
        {
            var effectivePermissions = ComputeEffectivePermissionsAsync(userContext, cancellationToken);

            return effectivePermissions.Contains(permission);
        }

        /// <inheritdoc />
        public PermissionSource GetPermissionSourceAsync(
            UserContext userContext,
            string permission,
            CancellationToken cancellationToken = default)
        {
            var hasFromRole = false;
            var hasFromOverride = false;

            // Check role permissions
            foreach (var role in userContext.Roles)
            {
                var rolePermissions = GetRolePermissions(role);
                if (rolePermissions.Contains(permission))
                {
                    hasFromRole = true;
                    break;
                }
            }

            /*
            // Check user-specific overrides
            var overrides = await _overrideRepository.GetUserOverridesAsync(
                userContext.UserId,
                userContext.AccountId,
                cancellationToken);

            hasFromOverride = overrides.Any(o => o.Permission == permission);
            */

            // Placeholder until override support is enabled
            hasFromOverride = true;

            if (hasFromRole && hasFromOverride)
            {
                return PermissionSource.Both;
            }

            if (hasFromRole)
            {
                return PermissionSource.Role;
            }

            if (hasFromOverride)
            {
                return PermissionSource.Override;
            }

            return PermissionSource.None;
        }

        /// <summary>
        /// Gets the base permissions associated with a specific user role.
        /// </summary>
        /// <param name="role">
        /// The role for which to retrieve permissions.
        /// </param>
        /// <returns>
        /// A set of permission identifiers granted by the specified role.
        /// </returns>
        public HashSet<string> GetRolePermissions(UserRoleType role)
        {
            return role switch
            {
                UserRoleType.Admin => GetAdminPermissions(),
                UserRoleType.AccountManager => GetAccountManagerPermissions(),
                UserRoleType.CSM => GetCSMPermissions(),
                UserRoleType.MainClient => GetMainClientPermissions(),
                UserRoleType.InvoicingClient => GetInvoicingClientPermissions(),
                UserRoleType.OperationsClient => GetOperationsClientPermissions(),
                _ => new HashSet<string>()
            };
        }

        /// <summary>
        /// Gets the full set of administrative permissions.
        /// </summary>
        /// <returns>
        /// A set of permission identifiers granted to administrators.
        /// </returns>
        private static HashSet<string> GetAdminPermissions()
        {
            return new HashSet<string>
            {
                // Administrative View - ALL
                Permissions.Administrative.View.AccountName,
                Permissions.Administrative.View.Account,
                Permissions.Administrative.View.Users,
                Permissions.Administrative.View.UserEmail,
                Permissions.Administrative.View.Timezone,
                Permissions.Administrative.View.Address,
                Permissions.Administrative.View.AccountStatus,
                Permissions.Administrative.View.Products,
                Permissions.Administrative.View.Orders,
                Permissions.Administrative.View.AuditLog,

                // Administrative Update - ALL
                Permissions.Administrative.Update.AccountName,
                Permissions.Administrative.Update.Account,
                Permissions.Administrative.Update.AccountType,
                Permissions.Administrative.Update.Timezone,
                Permissions.Administrative.Update.Address,
                Permissions.Administrative.Update.AccountStatus,
                Permissions.Administrative.Update.UserEmail,

                // Financial View - ALL
                Permissions.Financial.View.Currency,
                Permissions.Financial.View.VatNumber,
                Permissions.Financial.View.BillingEmail,
                Permissions.Financial.View.BillingType,

                // Financial Update - ALL
                Permissions.Financial.Update.Currency,
                Permissions.Financial.Update.VatNumber,
                Permissions.Financial.Update.BillingEmail,
                Permissions.Financial.Update.BillingType,
                Permissions.Financial.Update.NotificationEmail,
            };
        }

        /// <summary>
        /// Gets permissions for the Account Manager role.
        /// </summary>
        /// <remarks>
        /// Account Managers have the same permissions as Administrators,
        /// but access is restricted to assigned accounts and enforced
        /// by the validation layer.
        /// </remarks>
        private static HashSet<string> GetAccountManagerPermissions()
        {
            return GetAdminPermissions();
        }

        /// <summary>
        /// Gets permissions for the Customer Success Manager (CSM) role.
        /// </summary>
        private static HashSet<string> GetCSMPermissions()
        {
            return new HashSet<string>
            {
                // Administrative View - ALL
                Permissions.Administrative.View.AccountName,
                Permissions.Administrative.View.Account,
                Permissions.Administrative.View.Users,
                Permissions.Administrative.View.UserEmail,
                Permissions.Administrative.View.Timezone,
                Permissions.Administrative.View.Address,
                Permissions.Administrative.View.AccountStatus,
                Permissions.Administrative.View.Products,
                Permissions.Administrative.View.Orders,
                Permissions.Administrative.View.AuditLog,

                // Administrative Update - LIMITED
                Permissions.Administrative.Update.AccountName,
                Permissions.Administrative.Update.Account,
                Permissions.Administrative.Update.Timezone,
                Permissions.Administrative.Update.Address,
                Permissions.Administrative.Update.UserEmail,

                // Financial View - ALL
                Permissions.Financial.View.Currency,
                Permissions.Financial.View.VatNumber,
                Permissions.Financial.View.BillingEmail,
                Permissions.Financial.View.BillingType,
            };
        }

        /// <summary>
        /// Gets permissions for the Main Client role.
        /// </summary>
        private static HashSet<string> GetMainClientPermissions()
        {
            return new HashSet<string>
            {
                Permissions.Administrative.View.AccountName,
                Permissions.Administrative.View.Account,
                Permissions.Administrative.View.Users,
                Permissions.Administrative.View.UserEmail,
                Permissions.Administrative.View.Timezone,
                Permissions.Administrative.View.Address,
                Permissions.Administrative.View.AccountStatus,
                Permissions.Administrative.View.Products,
                Permissions.Administrative.View.Orders,

                Permissions.Administrative.Update.UserEmail,

                Permissions.Financial.View.Currency,
                Permissions.Financial.View.VatNumber,
                Permissions.Financial.View.BillingEmail,
                Permissions.Financial.View.BillingType,
            };
        }

        /// <summary>
        /// Gets permissions for the Invoicing Client role.
        /// </summary>
        private static HashSet<string> GetInvoicingClientPermissions()
        {
            return new HashSet<string>
            {
                Permissions.Administrative.View.AccountName,
                Permissions.Administrative.View.Account,
                Permissions.Administrative.View.Users,
                Permissions.Administrative.View.UserEmail,
                Permissions.Administrative.View.Timezone,
                Permissions.Administrative.View.Address,
                Permissions.Administrative.View.AccountStatus,
                Permissions.Administrative.View.Products,
                Permissions.Administrative.View.Orders,

                Permissions.Financial.View.Currency,
                Permissions.Financial.View.VatNumber,
                Permissions.Financial.View.BillingEmail,
                Permissions.Financial.View.BillingType,
            };
        }

        /// <summary>
        /// Gets permissions for the Operations Client role.
        /// </summary>
        private static HashSet<string> GetOperationsClientPermissions()
        {
            return new HashSet<string>
            {
                Permissions.Administrative.View.AccountName,
                Permissions.Administrative.View.Account,
                Permissions.Administrative.View.Users,
                Permissions.Administrative.View.UserEmail,
                Permissions.Administrative.View.Timezone,
                Permissions.Administrative.View.Address,
                Permissions.Administrative.View.AccountStatus,
                Permissions.Administrative.View.Products,
                Permissions.Administrative.View.Orders,
            };
        }

        /// <summary>
        /// Builds a failed <see cref="RoleValidationResult"/> from a single hard-stop message.
        /// Used for structural errors (E-01, E-02) that prevent further rule evaluation.
        /// </summary>
        /// <param name="message">The error message to include.</param>
        /// <returns>A <see cref="RoleValidationResult"/> with <c>IsValid = false</c>.</returns>
        private static RoleValidationResult Fail(Dictionary<string, string> message) =>
            new ()
            {
                IsValid = false,
                ValidationMessages = message,
                AllowedCombinations = SuggestedCombinations,
            };
    }
}
