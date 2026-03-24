// <copyright file="UserStatus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the status of a user account, indicating whether the user is active or inactive.
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// Indicates that the user is active and has access to the system.
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        Active = 1,

        /// <summary>
        /// Indicates that the user is inactive and does not have access to the system.
        /// </summary>
        [EnumMember(Value = "INACTIVE")]
        InActive = 2,
    }
}
