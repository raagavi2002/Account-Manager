// <copyright file="FieldPermissionResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Results
{
    /// <summary>
    /// Result of field-level permission validation.
    /// </summary>
    public class FieldPermissionResult
    {
        /// <summary>
        /// Gets the name of the field being evaluated for permission validation.
        /// </summary>
        required public string FieldName { get; init; }

        /// <summary>
        /// Gets a value indicating whether the required permission was granted for the field.
        /// </summary>
        public bool IsGranted { get; init; }

        /// <summary>
        /// Gets the permission key required to update the field.
        /// </summary>
        required public string RequiredPermission { get; init; }

        /// <summary>
        /// Gets the reason the permission was denied, if applicable.
        /// </summary>
        public string? DenialReason { get; init; }

    }
}
