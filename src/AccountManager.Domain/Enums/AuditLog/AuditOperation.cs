// <copyright file="AuditOperation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums.AuditLog
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the set of operations that can be recorded in the audit log.
    /// </summary>
    public enum AuditOperation
    {
        /// <summary>
        /// Indicates that a new account was created.
        /// </summary>
        [EnumMember(Value = "ACCOUNT_CREATED")]
        AccountCreated,

        /// <summary>
        /// Indicates that an existing account was updated.
        /// </summary>
        [EnumMember(Value = "ACCOUNT_UPDATED")]
        AccountUpdated,

        /// <summary>
        /// Indicates that the status of an account was changed.
        /// </summary>
        [EnumMember(Value = "ACCOUNT_STATUS_CHANGED")]
        AccountStatusChanged,

        /// <summary>
        /// Indicates that an account was archived.
        /// </summary>
        [EnumMember(Value = "ACCOUNT_ARCHIVED")]
        AccountArchived,

        /// <summary>
        /// Indicates that account ownership was transferred.
        /// </summary>
        [EnumMember(Value = "ACCOUNT_OWNERSHIP_TRANSFERRED")]
        AccountOwnershipTransferred,

        /// <summary>
        /// Indicates that an account was linked to another entity.
        /// </summary>
        [EnumMember(Value = "ACCOUNT_LINKED")]
        AccountLinked,

        /// <summary>
        /// Indicates that an account was unlinked from another entity.
        /// </summary>
        [EnumMember(Value = "ACCOUNT_UNLINKED")]
        AccountUnlinked,

        /// <summary>
        /// Indicates that a new user was created.
        /// </summary>
        [EnumMember(Value = "USER_CREATED")]
        UserCreated,

        /// <summary>
        /// Indicates that an existing user was updated.
        /// </summary>
        [EnumMember(Value = "USER_UPDATED")]
        UserUpdated,

        /// <summary>
        /// Indicates that a user was activated.
        /// </summary>
        [EnumMember(Value = "USER_ACTIVATED")]
        UserActivated,

        /// <summary>
        /// Indicates that a user was deactivated.
        /// </summary>
        [EnumMember(Value = "USER_DEACTIVATED")]
        UserDeactivated,

        /// <summary>
        /// Indicates that user roles were updated.
        /// </summary>
        [EnumMember(Value = "USER_ROLES_UPDATED")]
        UserRolesUpdated,

        /// <summary>
        /// Indicates that a user logged in.
        /// </summary>
        [EnumMember(Value = "USER_LOGGED_IN")]
        UserLoggedIn,

        /// <summary>
        /// Indicates that a user has been retrieved.
        /// </summary>
        [EnumMember(Value = "USER_RETRIEVED")]
        UserRetrieved,

        /// <summary>
        /// Indicates that the permissions cache was invalidated.
        /// </summary>
        [EnumMember(Value = "PERMISSIONS_CACHE_INVALIDATED")]
        PermissionsCacheInvalidated,

        /// <summary>
        /// Indicates that a bulk import operation was completed.
        /// </summary>
        [EnumMember(Value = "BULK_IMPORT_COMPLETED")]
        BulkImportCompleted,
    }
}
