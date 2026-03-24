// <copyright file="AuditEntityType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums.AuditLog
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the set of entity types that can be audited.
    /// </summary>
    public enum AuditEntityType
    {
        /// <summary>
        /// Represents an account entity.
        /// </summary>
        [EnumMember(Value = "ACCOUNT")]
        Account,

        /// <summary>
        /// Represents a user entity.
        /// </summary>
        [EnumMember(Value = "USER")]
        User,

        /// <summary>
        /// Represents an account relationship entity.
        /// </summary>
        [EnumMember(Value = "ACCOUNT_RELATIONSHIP")]
        AccountRelationship,

        /// <summary>
        /// Represents the permission cache entity.
        /// </summary>
        [EnumMember(Value = "PERMISSION_CACHE")]
        PermissionCache,

        /// <summary>
        /// Represents a bulk import entity.
        /// </summary>
        [EnumMember(Value = "BULK_IMPORT")]
        BulkImport,
    }
}
