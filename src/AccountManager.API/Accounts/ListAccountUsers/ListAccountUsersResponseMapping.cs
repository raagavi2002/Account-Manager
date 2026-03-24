// <copyright file="ListAccountUsersResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.ListAccountUsers
{
    using AccountManager.Application.Queries.ListAccountUsersQuery;
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile for account-users API response mapping.
    /// </summary>
    public class ListAccountUsersResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAccountUsersResponseMapping"/> class.
        /// </summary>
        public ListAccountUsersResponseMapping()
        {
            CreateMap<ListAccountUsersQueryResponse, ListAccountUsersAPIResponse>();
        }
    }
}
