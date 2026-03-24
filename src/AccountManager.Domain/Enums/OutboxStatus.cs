// <copyright file="OutboxStatus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the status of an outbox message used by the outbox pattern.
    /// </summary>
    public enum OutboxStatus
    {
        /// <summary>
        /// The message is created and waiting to be published.
        /// </summary>
        [EnumMember(Value = "PENDING")]
        Pending = 1,

        /// <summary>
        /// The message was successfully published.
        /// </summary>
        [EnumMember(Value = "PUBLISHED")]
        Published = 2,

        /// <summary>
        /// Publishing the message failed.
        /// </summary>
        [EnumMember(Value = "FAILED")]
        Failed = 3,
    }
}
