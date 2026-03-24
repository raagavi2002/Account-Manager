// <copyright file="GetAccountProductsResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountProducts
{
    using AccountManager.Application.Queries.GetAccountProductsQuery;
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile for account-products API response mapping.
    /// </summary>
    public class GetAccountProductsResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountProductsResponseMapping"/> class.
        /// </summary>
        public GetAccountProductsResponseMapping()
        {
            CreateMap<GetAccountProductsQueryResponse, GetAccountProductsAPIResponse>();
        }
    }
}
