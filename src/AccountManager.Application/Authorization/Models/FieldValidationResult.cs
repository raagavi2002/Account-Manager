// <copyright file="FieldValidationResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Models
{
    using AccountManager.Domain.Results;

    /// <summary>
    /// Consolidated result for field validation.
    /// </summary>
    public class FieldValidationResult
    {
        /// <summary>
        /// Gets a value indicating whether all field-level permission checks were successful.
        /// </summary>
        public bool IsValid { get; init; }

        /// <summary>
        /// Gets the collection of individual field permission evaluation results.
        /// </summary>
        public List<FieldPermissionResult> FieldResults { get; init; } = new();

        /// <summary>
        /// Gets the list of field names for which permission was denied.
        /// </summary>
        public List<string> DeniedFields { get; init; } = new();

        /// <summary>
        /// Builds a human-readable error message describing the missing permissions.
        /// </summary>
        /// <returns>
        /// An error message listing missing required permissions,
        /// or an empty string if all permissions are granted.
        /// </returns>
        public string GetErrorMessage()
        {
            if (IsValid)
            {
                return string.Empty;
            }

            var deniedPermissions = FieldResults
                .Where(f => !f.IsGranted)
                .Select(f => f.RequiredPermission)
                .Distinct();

            return $"Missing required permissions: {string.Join(", ", deniedPermissions)}";
        }

        /// <summary>
        /// Gets a dictionary of field-level validation errors keyed by field name.
        /// </summary>
        /// <returns>
        /// A dictionary where the key is the field name and the value is the denial reason.
        /// </returns>
        public Dictionary<string, string> GetFieldErrors()
        {
            return FieldResults
                .Where(f => !f.IsGranted)
                .ToDictionary(f => f.FieldName, f => f.DenialReason ?? "Permission denied");
        }
    }
}
