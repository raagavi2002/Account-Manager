/// <copyright file="ValidateAccountHierarchyResponse.cs" company="PlaceholderCompany">
/// Copyright (c) PlaceholderCompany. All rights reserved.
/// </copyright>

namespace AccountManager.API.Accounts.Validate_Account_Hierarchy
{
    using AccountManager.Domain.DTO;

    /// <summary>
    /// Represents the endpoint response if the account hierarachy has been validated.
    /// </summary>
    public class ValidateAccountHierarchyEndpointResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the hierarchy is valid.
        /// True if the hierarchy is valid; otherwise, false.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the list of validation messages and errors encountered during validation.
        /// </summary>
        public List<string> ValidationMessages { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the summary information for the head account.
        /// </summary>
        public AccountDto? HeadAccountInfo { get; set; }

        /// <summary>
        /// Gets or sets the summary information for the sub account.
        /// </summary>
        public AccountDto? SubAccountInfo { get; set; }
    }
}
