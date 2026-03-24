// <copyright file="GetTimezoneQueryResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Queries.GetTimezoneQuery
{
    /// <summary>
    /// Represents the response containing a list of timezones.
    /// </summary>
    public class GetTimezoneQueryResponse
    {
        /// <summary>
        /// Gets or sets the list of timezones.
        /// </summary>
        public List<Domain.DTO.TimezoneDto> Timezones { get; set; } = new ();
    }
}
