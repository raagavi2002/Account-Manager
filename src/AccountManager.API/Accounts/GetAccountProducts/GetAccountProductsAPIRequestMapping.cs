// <copyright file="GetAccountProductsAPIRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountProducts
{
    using AccountManager.Application.Queries.GetAccountProductsQuery;

    /// <summary>
    /// AutoMapper profile for account-products API request mapping.
    /// </summary>
    public class GetAccountProductsAPIRequestMapping : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountProductsAPIRequestMapping"/> class.
        /// </summary>
        public GetAccountProductsAPIRequestMapping()
        {
            CreateMap<GetAccountProductsAPIRequest, GetAccountProductsQueryRequest>();
        }
    }
}
