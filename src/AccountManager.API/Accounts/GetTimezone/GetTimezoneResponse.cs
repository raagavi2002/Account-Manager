// <copyright file="GetTimezoneResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetTimezone
{
    using AccountManager.Domain.DTO;

    /// <summary>
    /// Represents the response containing a list of supported timezones.
    /// </summary>
    public class GetTimezoneResponse
    {
        /// <summary>
        /// Gets or sets the list of supported timezones.
        /// </summary>
        public List<TimezoneDto> Timezones { get; set; } = new ();
    }
}
