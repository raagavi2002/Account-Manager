// <copyright file="ListAccountUsersQueryResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.ListAccountUsersQuery
{
    using AccountManager.Domain.DTO;

    /// <summary>
    /// Represents the paginated response for account users.
    /// </summary>
    public class ListAccountUsersQueryResponse
    {
        /// <summary>
        /// Gets or sets the list of users.
        /// </summary>
        public List<UserDto> Users { get; set; } = new ();

        /// <summary>
        /// Gets or sets the total number of matching users.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int Page { get; set; }

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
