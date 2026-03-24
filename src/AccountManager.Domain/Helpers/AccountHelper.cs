// <copyright file="AccountHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AccountManager.Domain.Helpers
{
    using System;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.ValueObjects;

    /// <summary>
    /// Provides helper methods for account-related operations.
    /// </summary>
    public static class AccountHelper
    {
        /// <summary>
        /// Validates whether an address contains all required information.
        /// </summary>
        /// <param name="address">The address to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="address"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when any required address field is null, empty, or whitespace.
        /// </exception>
        /// <remarks>
        /// An address is considered valid if all required properties
        /// (Street, City, State, PostalCode, Country) are not null, empty, or whitespace.
        /// The optional Street2 property is not validated.
        /// </remarks>
        public static void ValidateAddress(AddressDto address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Address cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(address.Street))
            {
                throw new ArgumentException("Street is required and cannot be empty.", nameof(address));
            }

            if (string.IsNullOrWhiteSpace(address.City))
            {
                throw new ArgumentException("City is required and cannot be empty.", nameof(address));
            }

            if (string.IsNullOrWhiteSpace(address.State))
            {
                throw new ArgumentException("State is required and cannot be empty.", nameof(address));
            }

            if (string.IsNullOrWhiteSpace(address.PostalCode))
            {
                throw new ArgumentException("Postal code is required and cannot be empty.", nameof(address));
            }

            if (string.IsNullOrWhiteSpace(address.Country))
            {
                throw new ArgumentException("Country is required and cannot be empty.", nameof(address));
            }
        }

        /// <summary>
        /// Validates whether an address contains all required information.
        /// </summary>
        /// <param name="address">The address to validate.</param>
        /// <returns>
        /// <c>true</c> if the address is valid and contains all required fields; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method does not throw exceptions. Use <see cref="ValidateAddress"/> for validation with exceptions.
        /// </remarks>
        public static bool IsValidAddress(AddressDto address)
        {
            try
            {
                ValidateAddress(address);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
