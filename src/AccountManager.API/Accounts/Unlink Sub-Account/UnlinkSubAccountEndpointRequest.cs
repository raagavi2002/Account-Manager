// <copyright file="UnlinkSubAccountEndpointRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Unlink_Sub_Account
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// API request the unlinking of a sub-account
    /// from a head account.
    /// </summary>
    public class UnlinkSubAccountEndpointRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier of the head account
        /// from which the sub-account will be unlinked.
        /// </summary>
        [FromRoute]
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the sub-account
        /// that is to be unlinked.
        /// </summary>
        [FromRoute]
        public Guid SubAccountId { get; set; }

        /// <summary>
        /// Gets or sets the reason for unlinking the sub-account.
        /// </summary>
        required public string Reason { get; set; }

        public string RequiredPermission => Permissions.Administrative.Update.Account;

        string? IRequirePermission.AccountId => HeadAccountId.ToString();
    }
}
