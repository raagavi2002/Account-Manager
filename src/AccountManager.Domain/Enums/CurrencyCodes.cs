// <copyright file="CurrencyCodes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the supported currency codes in the system.
    /// </summary>
    public enum CurrencyCodes
    {
        /// <summary>
        /// United States Dollar.
        /// </summary>
        [EnumMember(Value = "USD")]
        USD = 1,

        /// <summary>
        /// Euro.
        /// </summary>
        [EnumMember(Value = "EUR")]
        EUR = 2,

        /// <summary>
        /// British Pound Sterling.
        /// </summary>
        [EnumMember(Value = "GBP")]
        GBP = 3,

        /// <summary>
        /// Canadian Dollar.
        /// </summary>
        [EnumMember(Value = "CAD")]
        CAD = 4,

        /// <summary>
        /// Australian Dollar.
        /// </summary>
        [EnumMember(Value = "AUD")]
        AUD = 5,

        /// <summary>
        /// Japanese Yen.
        /// </summary>
        [EnumMember(Value = "JPY")]
        JPY = 6,

        /// <summary>
        /// Chinese Yuan.
        /// </summary>
        [EnumMember(Value = "CNY")]
        CNY = 7,

        /// <summary>
        /// Indian Rupee.
        /// </summary>
        [EnumMember(Value = "INR")]
        INR = 8,

        /// <summary>
        /// Swiss Franc.
        /// </summary>
        [EnumMember(Value = "CHF")]
        CHF = 9,

        /// <summary>
        /// New Zealand Dollar.
        /// </summary>
        [EnumMember(Value = "NZD")]
        NZD = 10,
    }
}
