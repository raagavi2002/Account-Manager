// <copyright file="AccountStatus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the lifecycle status of an account.
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        /// The account has been created but is not yet active.
        /// </summary>
        [EnumMember(Value = "PREACTIVE")]
        PreActive = 1,

        /// <summary>
        /// The account is currently active and operational.
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        Active = 2,

        /// <summary>
        /// The account is inactive but may be reactivated in the future.
        /// </summary>
        [EnumMember(Value = "INACTIVE")]
        Inactive = 3,

        /// <summary>
        /// The account has been archived and is no longer in active use.
        /// </summary>
        [EnumMember(Value = "ARCHIVED")]
        Archive = 4,

        /// <summary>
        /// The account has been explicitly deactivated and cannot be used.
        /// </summary>
        [EnumMember(Value = "DEACTIVATE")]
        Deactivate = 5,
    }
}
