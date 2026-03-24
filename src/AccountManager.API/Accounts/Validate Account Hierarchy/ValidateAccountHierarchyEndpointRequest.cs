// <copyright file="ValidateAccountHierarchyRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Validate_Account_Hierarchy
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;
    /// <summary>
    /// Represents the request payload for validating an account hierarchy.
    /// Used to check whether a given account can be designated as a head account
    /// and another as its sub-account.
    /// </summary>
    public class ValidateAccountHierarchyEndpointRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account that will become the head account.
        /// </summary>
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the account that will become the sub-account.
        /// </summary>
        public Guid SubAccountId { get; set; }

        public string RequiredPermission => Permissions.Administrative.View.Account;

        string? IRequirePermission.AccountId => HeadAccountId.ToString();
    }
}
