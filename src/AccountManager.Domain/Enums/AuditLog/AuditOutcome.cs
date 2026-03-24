// <copyright file="AuditOutcome.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Runtime.Serialization;

namespace AccountManager.Domain.Enums.AuditLog
{
    /// <summary>
    /// Defines the possible outcomes of an audited operation.
    /// </summary>
    public enum AuditOutcome
    {
        /// <summary>
        /// Indicates that the operation completed successfully.
        /// </summary>
        [EnumMember(Value = "SUCCESS")]
        Success,

        /// <summary>
        /// Indicates that the operation failed.
        /// </summary>
        [EnumMember(Value = "FAILURE")]
        Failure,

        /// <summary>
        /// Indicates that the operation completed with partial success.
        /// </summary>
        [EnumMember(Value = "PARTIAL_SUCCESS")]
        PartialSuccess,
    }
}
