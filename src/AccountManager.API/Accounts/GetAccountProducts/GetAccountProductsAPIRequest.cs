// <copyright file="GetAccountProductsAPIRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountProducts
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;

    /// <summary>
    /// Represents a request to retrieve products associated with an account.
    /// </summary>
    public class GetAccountProductsAPIRequest : IRequirePermission
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
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <inheritdoc/>
        public string RequiredPermission => Permissions.Administrative.View.Products;

        /// <inheritdoc/>
        string? IRequirePermission.AccountId => AccountId.ToString();
    }
}
