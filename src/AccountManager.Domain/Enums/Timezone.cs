// <copyright file="Timezone.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents 25 common IANA time zones with EnumMember attributes.
    /// </summary>
    /// <remarks>
    /// Each enum member is decorated with an <see cref="EnumMemberAttribute"/>
    /// that maps to the canonical IANA time zone identifier.
    /// </remarks>
    public enum TimeZone
    {
        /// <summary>
        /// Coordinated Universal Time (UTC).
        /// </summary>
        [EnumMember(Value = "UTC")]
        UTC = 1,

        /// <summary>
        /// Pacific Time (Los Angeles, USA). IANA: America/Los_Angeles
        /// </summary>
        [EnumMember(Value = "America/Los_Angeles")]
        America_Los_Angeles = 2,

        /// <summary>
        /// Mountain Time (Denver, USA). IANA: America/Denver
        /// </summary>
        [EnumMember(Value = "America/Denver")]
        America_Denver = 3,

        /// <summary>
        /// Central Time (Chicago, USA). IANA: America/Chicago
        /// </summary>
        [EnumMember(Value = "America/Chicago")]
        America_Chicago = 4,

        /// <summary>
        /// Eastern Time (New York, USA). IANA: America/New_York
        /// </summary>
        [EnumMember(Value = "America/New_York")]
        America_New_York = 5,

        /// <summary>
        /// Atlantic Time (Halifax, Canada). IANA: America/Halifax
        /// </summary>
        [EnumMember(Value = "America/Halifax")]
        America_Halifax = 6,

        /// <summary>
        /// Greenwich Mean Time (London, UK). IANA: Europe/London
        /// </summary>
        [EnumMember(Value = "Europe/London")]
        Europe_London = 7,

        /// <summary>
        /// Central European Time (Berlin, Germany). IANA: Europe/Berlin
        /// </summary>
        [EnumMember(Value = "Europe/Berlin")]
        Europe_Berlin = 8,

        /// <summary>
        /// Eastern European Time (Athens, Greece). IANA: Europe/Athens
        /// </summary>
        [EnumMember(Value = "Europe/Athens")]
        Europe_Athens = 9,

        /// <summary>
        /// Moscow Standard Time (Moscow, Russia). IANA: Europe/Moscow
        /// </summary>
        [EnumMember(Value = "Europe/Moscow")]
        Europe_Moscow = 10,

        /// <summary>
        /// India Standard Time (Asia/Kolkata).
        /// </summary>
        [EnumMember(Value = "Asia/Kolkata")]
        Asia_Kolkata = 11,

        /// <summary>
        /// China Standard Time (Asia/Shanghai).
        /// </summary>
        [EnumMember(Value = "Asia/Shanghai")]
        Asia_Shanghai = 12,

        /// <summary>
        /// Japan Standard Time (Asia/Tokyo).
        /// </summary>
        [EnumMember(Value = "Asia/Tokyo")]
        Asia_Tokyo = 13,

        /// <summary>
        /// Korea Standard Time (Asia/Seoul).
        /// </summary>
        [EnumMember(Value = "Asia/Seoul")]
        Asia_Seoul = 14,

        /// <summary>
        /// Singapore Standard Time (Asia/Singapore).
        /// </summary>
        [EnumMember(Value = "Asia/Singapore")]
        Asia_Singapore = 15,

        /// <summary>
        /// Hong Kong Time (Asia/Hong_Kong).
        /// </summary>
        [EnumMember(Value = "Asia/Hong_Kong")]
        Asia_Hong_Kong = 16,

        /// <summary>
        /// Australian Eastern Time (Sydney). IANA: Australia/Sydney
        /// </summary>
        [EnumMember(Value = "Australia/Sydney")]
        Australia_Sydney = 17,

        /// <summary>
        /// Australian Western Time (Perth). IANA: Australia/Perth
        /// </summary>
        [EnumMember(Value = "Australia/Perth")]
        Australia_Perth = 18,

        /// <summary>
        /// New Zealand Standard Time (Auckland). IANA: Pacific/Auckland
        /// </summary>
        [EnumMember(Value = "Pacific/Auckland")]
        Pacific_Auckland = 19,

        /// <summary>
        /// Hawaii Standard Time (Honolulu, USA). IANA: Pacific/Honolulu
        /// </summary>
        [EnumMember(Value = "Pacific/Honolulu")]
        Pacific_Honolulu = 20,

        /// <summary>
        /// Alaska Standard Time (Anchorage, USA). IANA: America/Anchorage
        /// </summary>
        [EnumMember(Value = "America/Anchorage")]
        America_Anchorage = 21,

        /// <summary>
        /// Brasilia Time (São Paulo, Brazil). IANA: America/Sao_Paulo
        /// </summary>
        [EnumMember(Value = "America/Sao_Paulo")]
        America_Sao_Paulo = 22,

        /// <summary>
        /// Argentina Time (Buenos Aires). IANA: America/Argentina/Buenos_Aires
        /// </summary>
        [EnumMember(Value = "America/Argentina/Buenos_Aires")]
        America_Argentina_Buenos_Aires = 23,

        /// <summary>
        /// South Africa Standard Time (Johannesburg). IANA: Africa/Johannesburg
        /// </summary>
        [EnumMember(Value = "Africa/Johannesburg")]
        Africa_Johannesburg = 24,

        /// <summary>
        /// Egypt Standard Time (Cairo). IANA: Africa/Cairo
        /// </summary>
        [EnumMember(Value = "Africa/Cairo")]
        Africa_Cairo = 25,
    }
}
