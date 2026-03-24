// <copyright file="GetAccountProductsQueryResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetAccountProductsQuery
{
    using AccountManager.Domain.DTO;

    /// <summary>
    /// Represents the paginated response for account-product associations.
    /// </summary>
    public class GetAccountProductsQueryResponse
    {
        /// <summary>
        /// Gets or sets the product associations.
        /// </summary>
        public List<ProductAssociationDto> Products { get; set; } = new ();

        /// <summary>
        /// Gets or sets the total number of matching products.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether more pages are available.
        /// </summary>
        public bool HasMore { get; set; }
    }
}
