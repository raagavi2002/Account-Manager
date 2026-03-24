// <copyright file="GetAccountDetailsAPIRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountDetails
{
    using AccountManager.Application.Queries.GetAccountDetailsQuery;

    /// <summary>
    /// AutoMapper profile for mapping API requests to application query requests.
    /// </summary>
    public class GetAccountDetailsAPIRequestMapping : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountDetailsAPIRequestMapping"/> class.
        /// Configures mapping from <see cref="GetAccountDetailsAPIRequest"/> to <see cref="GetAccountDetailsQueryRequest"/>.
        /// </summary>
        public GetAccountDetailsAPIRequestMapping()
        {
            // Map from API request to Application query request
            CreateMap<GetAccountDetailsAPIRequest, GetAccountDetailsQueryRequest>();
        }
    }
}
