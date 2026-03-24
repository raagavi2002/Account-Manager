// <copyright file="UserType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the different types of users supported by the system.
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// Represents an internal user of the organization.
        /// </summary>
        [EnumMember(Value = "INTERNALUSER")]
        InternalUser = 1,

        /// <summary>
        /// Represents a client or external user.
        /// </summary>
        [EnumMember(Value = "CLIENTUSER")]
        ClientUser = 2,
    }
}
