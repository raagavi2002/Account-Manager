// <copyright file="FieldPermissionValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Authorization
{
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Authorization.Models;
    using AccountManager.Domain.Enums.Authorization;
    using AccountManager.Domain.Results;
    using AccountManager.Shared.Logging;

    /// <summary>
    /// Concrete implementation of <see cref="IFieldPermissionValidator"/>.
    /// Validates field-level permissions for update operations by comparing
    /// original vs updated objects and checking user permissions for each changed field.
    /// </summary>
    public class FieldPermissionValidator : IFieldPermissionValidator
    {
        private static readonly Dictionary<string, string> FieldPermissionMap = new ()
        {
            // Administrative fields
            ["accountname"] = Permissions.Administrative.Update.AccountName,
            ["accounttype"] = Permissions.Administrative.Update.AccountType,
            ["timezone"] = Permissions.Administrative.Update.Timezone,
            ["address"] = Permissions.Administrative.Update.Address,
            ["status"] = Permissions.Administrative.Update.AccountStatus,
            ["useremail"] = Permissions.Administrative.Update.UserEmail,

            // Financial fields
            ["currency"] = Permissions.Financial.Update.Currency,
            ["vatnumber"] = Permissions.Financial.Update.VatNumber,
            ["billingemail"] = Permissions.Financial.Update.BillingEmail,
            ["billingtype"] = Permissions.Financial.Update.BillingType,
            ["notificationemail"] = Permissions.Financial.Update.NotificationEmail,
        };

        private readonly IPermissionCalculator permissionCalculator;
        private readonly IApplogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldPermissionValidator"/> class.
        /// </summary>
        /// <param name="permissionCalculator"> An instance of the permission calculator.</param>
        /// <param name="logger"></param>
        public FieldPermissionValidator(IPermissionCalculator permissionCalculator, IApplogger logger)
        {
            this.permissionCalculator = permissionCalculator;
            this.logger = logger;
        }

        /// <summary>
        /// Validates permissions for all fields being updated.
        /// Compares original vs updated object and checks permission for each changed field.
        /// </summary>
        /// <typeparam name="T">The type of object being validated.</typeparam>
        /// <param name="userContext">The user context containing identity and permissions.</param>
        /// <param name="original">The original object before update.</param>
        /// <param name="updated">The updated object after changes.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>
        /// A <see cref="FieldValidationResult"/> containing:
        /// - <c>IsValid</c>: true if all field updates are permitted, false otherwise
        /// - <c>FieldResults</c>: detailed results for each field
        /// - <c>DeniedFields</c>: list of fields the user is not allowed to update
        /// </returns>
        public async Task<FieldValidationResult> ValidateFieldUpdatesAsync<T>(
            UserContext userContext,
            T original,
            T updated,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var fieldResults = new List<FieldPermissionResult>();
            var deniedFields = new List<string>();

            var changedFields = GetChangedFields(original, updated);

            if (!changedFields.Any())
            {
                logger.LogInformation("No fields changed for user {UserId}", userContext.UserId);
                return new FieldValidationResult { IsValid = true };
            }

            logger.LogInformation(
                "Validating {Count} changed fields for user {UserId}: {Fields}",
                changedFields.Count,
                userContext.UserId,
                string.Join(", ", changedFields));

            var effectivePermissions = permissionCalculator
                .ComputeEffectivePermissionsAsync(userContext, cancellationToken);

            foreach (var fieldName in changedFields)
            {
                var requiredPermission = GetRequiredPermissionForField(fieldName);

                if (requiredPermission == null)
                {
                    fieldResults.Add(new FieldPermissionResult
                    {
                        FieldName = fieldName,
                        IsGranted = true,
                        RequiredPermission = "None",
                    });
                    continue;
                }

                var isGranted = effectivePermissions.Contains(requiredPermission);

                fieldResults.Add(new FieldPermissionResult
                {
                    FieldName = fieldName,
                    IsGranted = isGranted,
                    RequiredPermission = requiredPermission,
                    DenialReason = isGranted ? null : $"Missing permission: {requiredPermission}",
                });

                if (!isGranted)
                {
                    deniedFields.Add(fieldName);
                    logger.LogError(
                        "Field update denied: {FieldName} requires {Permission} for user {UserId}",
                        fieldName,
                        requiredPermission,
                        userContext.UserId);
                }
            }

            var isValid = deniedFields.Count == 0;

            return new FieldValidationResult
            {
                IsValid = isValid,
                FieldResults = fieldResults,
                DeniedFields = deniedFields,
            };
        }

        /// <summary>
        /// Validates a single field update.
        /// </summary>
        /// <param name="userContext">The user context containing identity and permissions.</param>
        /// <param name="fieldName">The name of the field being updated.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>
        /// A <see cref="FieldPermissionResult"/> indicating:
        /// - <c>IsGranted</c>: whether the user has permission for the field
        /// - <c>RequiredPermission</c>: the permission string required
        /// - <c>DenialReason</c>: explanation if permission is denied
        /// </returns>
        public async Task<FieldPermissionResult> ValidateSingleFieldAsync(
            UserContext userContext,
            string fieldName,
            CancellationToken cancellationToken = default)
        {
            var requiredPermission = GetRequiredPermissionForField(fieldName);

            if (requiredPermission == null)
            {
                return new FieldPermissionResult
                {
                    FieldName = fieldName,
                    IsGranted = true,
                    RequiredPermission = "None",
                };
            }

            var hasPermission = permissionCalculator.HasPermissionAsync(
                userContext,
                requiredPermission,
                cancellationToken);

            return new FieldPermissionResult
            {
                FieldName = fieldName,
                IsGranted = hasPermission,
                RequiredPermission = requiredPermission,
                DenialReason = hasPermission ? null : $"Missing permission: {requiredPermission}",
            };
        }

        /// <summary>
        /// Gets the required permission for a specific field.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>
        /// The permission string required to update the field, or <c>null</c> if no permission is required.
        /// </returns>
        public string? GetRequiredPermissionForField(string fieldName)
        {
            var normalizedFieldName = fieldName.ToLowerInvariant();
            return FieldPermissionMap.GetValueOrDefault(normalizedFieldName);
        }

        /// <summary>
        /// Determines which fields have changed between two objects.
        /// </summary>
        /// <typeparam name="T">The type of object being compared.</typeparam>
        /// <param name="original">The original object.</param>
        /// <param name="updated">The updated object.</param>
        /// <returns>
        /// A list of field names that differ between the original and updated objects.
        /// </returns>
        private static List<string> GetChangedFields<T>(T original, T updated)
            where T : class
        {
            var changedFields = new List<string>();
            var properties = typeof(T).GetProperties(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                var originalValue = property.GetValue(original);
                var updatedValue = property.GetValue(updated);

                if (!Equals(originalValue, updatedValue))
                {
                    changedFields.Add(property.Name);
                }
            }

            return changedFields;
        }
    }
}
