// <copyright file="ListAccountUsersAPIRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.ListAccountUsers
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;

    /// <summary>
    /// Represents a request to retrieve users associated with an account.
    /// </summary>
    public class ListAccountUsersAPIRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the account identifier from the route.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets an optional active-state filter.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets an optional role filter.
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets the permission required to perform this request.
        /// </summary>
        public string RequiredPermission => Permissions.Administrative.View.Users;

        /// <summary>
        /// Gets the account identifier associated with the permission requirement.
        /// </summary>
        string? IRequirePermission.AccountId => AccountId.ToString();
    }
}
