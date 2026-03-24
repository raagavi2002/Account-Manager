// <copyright file="AuditEntryCreatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data
{
    using System;
    using AccountManager.Domain.Enums.AuditLog;

    /// <summary>
    /// Represents the payload for an audit entry created event.
    /// Captures all relevant details of an audit log entry for event publishing.
    /// </summary>
    public class AuditEntryCreatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the audit log entry.
        /// </summary>
        public Guid AuditId { get; set; }

        /// <summary>
        /// Gets or sets the type of entity that was affected by the operation.
        /// </summary>
        public AuditEntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the entity affected by the operation.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of a related entity, if applicable.
        /// </summary>
        public Guid? RelatedEntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of operation performed (e.g., CREATE, UPDATE, DELETE).
        /// </summary>
        public AuditOperation OperationType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who performed the operation.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the operation occurred.
        /// </summary>
        public DateTime OccurredAtUtc { get; set; }

        /// <summary>
        /// Gets or sets the outcome of the operation (e.g., SUCCESS, FAILURE).
        /// </summary>
        public AuditOutcome Outcome { get; set; }

        /// <summary>
        /// Gets or sets the reason provided for the operation, if any.
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Gets or sets the previous state of the entity in JSON format.
        /// </summary>
        public string? BeforeState { get; set; }

        /// <summary>
        /// Gets or sets the new state of the entity in JSON format.
        /// </summary>
        public string? AfterState { get; set; }

        /// <summary>
        /// Gets or sets the list of fields that were changed during the operation in JSON format.
        /// </summary>
        public string? ChangedFields { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier for tracking related operations.
        /// </summary>
        public Guid CorrelationId { get; set; }
    }
}
