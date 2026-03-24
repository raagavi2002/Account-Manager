// <copyright file="GetAccountProductsQueryRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetAccountProductsQuery
{
    using MediatR;

    /// <summary>
    /// Represents a request to retrieve products associated with an account.
    /// </summary>
    public class GetAccountProductsQueryRequest : IRequest<GetAccountProductsQueryResponse>
    {
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets an optional filter for active/inactive associations.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the page number starting from 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;
    }
}
