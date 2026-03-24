// <copyright file="AddressDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Represents a postal address used for account-related data transfer.
    /// </summary>
    public class AddressDto
    {
        /// <summary>
        /// Gets or sets the primary street address.
        /// </summary>
        required public string Street { get; set; }

        /// <summary>
        /// Gets or sets the secondary street address, such as an apartment or suite number.
        /// </summary>
        public string? Street2 { get; set; }

        /// <summary>
        /// Gets or sets the city of the address.
        /// </summary>
        required public string City { get; set; }

        /// <summary>
        /// Gets or sets the state, province, or region of the address.
        /// </summary>
        required public string State { get; set; }

        /// <summary>
        /// Gets or sets the postal or ZIP code.
        /// </summary>
        required public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country of the address.
        /// </summary>
        required public string Country { get; set; }
    }
}
