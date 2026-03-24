// <copyright file="GetAccountDetailsAPIRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountDetails
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents a request to retrieve the details of an account by its unique identifier.
    /// </summary>
    public class GetAccountDetailsAPIRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account.
        /// </summary>
        public Guid AccountId { get; set; }

        public string RequiredPermission => Permissions.Administrative.View.Account;

        string? IRequirePermission.AccountId => AccountId.ToString();
    }
}
