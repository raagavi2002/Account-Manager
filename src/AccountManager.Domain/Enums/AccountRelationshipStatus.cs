// <copyright file="AccountRelationshipStatus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the possible states of an account relationship.
    /// </summary>
    /// <remarks>
    /// This enumeration is used to represent whether an account
    /// relationship is currently active or inactive within the system.
    /// </remarks>
    public enum AccountRelationshipStatus
    {
        /// <summary>
        /// Indicates that the account relationship is active.
        /// </summary>
        /// <remarks>
        /// An active relationship means that the linked accounts are
        /// currently associated and the relationship is in effect.
        /// </remarks>
        [EnumMember(Value = "ACTIVE")]
        Active = 1,

        /// <summary>
        /// Indicates that the account relationship is inactive.
        /// </summary>
        /// <remarks>
        /// An inactive relationship means that the linked accounts are
        /// no longer associated or the relationship has been disabled.
        /// </remarks>
        [EnumMember(Value = "INACTIVE")]
        Inactive = 2,
    }
}
