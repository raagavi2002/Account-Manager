// <copyright file="AuditLogEntry.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO.Audit_Log
{
    /// <summary>
    /// Represents an entry in the audit log, capturing details of user actions,
    /// affected resources, status, and changes made.
    /// </summary>
    public class AuditLogEntry
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who performed the action.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the action performed by the user (e.g., "Update", "Delete").
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the resource type affected by the action (e.g., "User", "Account").
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the resource affected by the action.
        /// </summary>
        public Guid ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the status of the action (e.g., "Success", "Failed").
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the list of changes made during the action.
        /// Each change includes the field name, old value, and new value.
        /// </summary>
        public List<(string fieldName, object oldValue, object newValue)> Changes { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the action occurred.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
