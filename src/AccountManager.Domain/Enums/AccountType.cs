// <copyright file="AccountType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the types of accounts available in the system.
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Represents a professional-level account.
        /// </summary>
        [EnumMember(Value = "PROFESSIONAL")]
        Professional = 1,

        /// <summary>
        /// Represents an enterprise-level account.
        /// </summary>
        [EnumMember(Value = "ENTERPRISE")]
        Enterprise = 2,

        /// <summary>
        /// Represents a corporate-level account.
        /// </summary>
        [EnumMember(Value = "CORPORATE")]
        Corporate = 3,
    }
}
