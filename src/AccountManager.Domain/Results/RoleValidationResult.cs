// <copyright file="RoleValidationResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Results
{
    /// <summary>
    /// Represents the outcome of a role combination validation check.
    /// </summary>
    public sealed class RoleValidationResult
    {
        /// <summary>
        /// Gets a value indicating whether the role combination is valid (OUT-01).
        /// </summary>
        public bool IsValid { get; init; }

        /// <summary>
        /// Gets the violation messages describing why the combination failed (OUT-02).
        /// Empty when <see cref="IsValid"/> is <c>true</c>.
        /// </summary>
        public Dictionary<string, string> ValidationMessages { get; init; } = [];

        /// <summary>
        /// Gets the suggested valid role combinations to guide the caller (OUT-03).
        /// Empty when <see cref="IsValid"/> is <c>true</c>.
        /// </summary>
        public List<string> AllowedCombinations { get; init; } = [];
    }
}
