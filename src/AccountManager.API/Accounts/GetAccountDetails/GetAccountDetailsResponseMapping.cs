// <copyright file="GetAccountDetailsResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountDetails
{
    using AccountManager.Application.Queries.GetAccountDetailsQuery;
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile for mapping application responses to API responses.
    /// </summary>
    public class GetAccountDetailsResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountDetailsResponseMapping"/> class.
        /// Configures AutoMapper mappings for account details responses.
        /// </summary>
        public GetAccountDetailsResponseMapping()
        {
            // Map from Application response to API response
            CreateMap<GetAccountDetailsQueryResponse, GetAccountDetailsAPIResponse>();
        }
    }
}
