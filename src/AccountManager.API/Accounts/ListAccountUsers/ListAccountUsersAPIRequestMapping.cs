// <copyright file="ListAccountUsersAPIRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.ListAccountUsers
{
    using AccountManager.Application.Queries.ListAccountUsersQuery;

    /// <summary>
    /// AutoMapper profile for account-users API request mapping.
    /// </summary>
    public class ListAccountUsersAPIRequestMapping : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAccountUsersAPIRequestMapping"/> class.
        /// </summary>
        public ListAccountUsersAPIRequestMapping()
        {
            CreateMap<ListAccountUsersAPIRequest, ListAccountUsersQueryRequest>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.Page));
        }
    }
}
