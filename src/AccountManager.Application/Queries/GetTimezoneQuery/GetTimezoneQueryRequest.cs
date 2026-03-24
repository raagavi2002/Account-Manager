// <copyright file="GetTimezoneQueryRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetTimezoneQuery
{
    using MediatR;

    /// <summary>
    /// Represents a request to retrieve the list of supported timezones.
    /// </summary>
    public class GetTimezoneQueryRequest : IRequest<GetTimezoneQueryResponse>
    {
    }
}
