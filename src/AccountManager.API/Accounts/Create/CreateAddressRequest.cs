// <copyright file="CreateAddressRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Create
{
    /// <summary>
    /// Represents a physicaladdress.
    /// </summary>
    public class CreateAddressRequest
    {
        /// <summary>
        /// Gets or sets the primary street address.
        /// </summary>
        required public string Street { get; set; }

        /// <summary>
        /// Gets or sets additional street address information,
        /// such as an apartment or suite number.
        /// </summary>
        public string? Street2 { get; set; }

        /// <summary>
        /// Gets or sets the city or locality.
        /// </summary>
        required public string City { get; set; }

        /// <summary>
        /// Gets or sets the state, province, or region.
        /// </summary>
        required public string State { get; set; }

        /// <summary>
        /// Gets or sets the postal or ZIP code.
        /// </summary>
        required public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        required public string Country { get; set; }
    }
}
