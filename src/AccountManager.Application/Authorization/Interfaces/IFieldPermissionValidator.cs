// <copyright file="IFieldPermissionValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Authorization.Interfaces
{
    using AccountManager.Application.Authorization.Models;
    using AccountManager.Domain.Results;

    /// <summary>
    /// Validates field-level permissions for update operations.
    /// Per SBR-ACM-010: "Field-Level Update Validation: For update operations, 
    /// verify permission for each specific field being changed".
    /// </summary>
    public interface IFieldPermissionValidator
    {
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
        /// - Success/failure of validation
        /// - List of fields validated
        /// - Any permission errors encountered.
        /// </returns>
        Task<FieldValidationResult> ValidateFieldUpdatesAsync<T>(
            UserContext userContext,
            T original,
            T updated,
            CancellationToken cancellationToken = default)
            where T : class;

        /// <summary>
        /// Gets the required permission for a specific field.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>
        /// The permission string required to update the field, or <c>null</c> if no permission is required.
        /// </returns>
        string? GetRequiredPermissionForField(string fieldName);

        /// <summary>
        /// Validates a single field update.
        /// </summary>
        /// <param name="userContext">The user context containing identity and permissions.</param>
        /// <param name="fieldName">The name of the field being updated.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>
        /// A <see cref="FieldPermissionResult"/> indicating:
        /// - Whether the user has permission for the field
        /// - The required permission (if any)
        /// - Error details if validation fails.
        /// </returns>
        Task<FieldPermissionResult> ValidateSingleFieldAsync(
            UserContext userContext,
            string fieldName,
            CancellationToken cancellationToken = default);
    }
}
