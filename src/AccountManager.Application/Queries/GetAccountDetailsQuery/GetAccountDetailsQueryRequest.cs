// <copyright file="GetAccountDetailsRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetAccountDetailsQuery
{
    using MediatR;

    /// <summary>
    /// Represents a request to retrieve account details by account identifier.
    /// </summary>
    public class GetAccountDetailsQueryRequest : IRequest<GetAccountDetailsQueryResponse>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account.
        /// </summary>
        required public Guid AccountId { get; set; }
    }
}
