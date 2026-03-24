// <copyright file="AuditLog.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Aggregates.AuditAggregate
{
    using System;
    using AccountManager.Domain.Enums.AuditLog;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents the AuditLog aggregate root - an immutable audit trail entry capturing
    /// details of operations performed within the Account Manager service.
    /// </summary>
    /// <remarks>
    /// The AuditLog aggregate maintains compliance with audit requirements by ensuring:
    /// - Immutability: Audit entries cannot be modified or deleted after creation
    /// - Complete State Capture: Both before and after states are captured for UPDATE operations
    /// - Timestamp Accuracy: All timestamps stored in UTC
    /// - User Attribution: Every operation is attributed to a user or system
    /// </remarks>
    public sealed class AuditLog
    {
        /// <summary>
        /// Gets the unique identifier for this audit log entry.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the type of entity that was affected by the operation.
        /// </summary>
        public AuditEntityType EntityType { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the entity affected by the operation.
        /// </summary>
        public Guid EntityId { get; private set; }

        /// <summary>
        /// Gets the identifier of a related entity, if applicable.
        /// </summary>
        public Guid? RelatedEntityId { get; private set; }

        /// <summary>
        /// Gets the type of operation performed (e.g., CREATE, UPDATE, DELETE).
        /// </summary>
        public AuditOperation OperationType { get; private set; }

        /// <summary>
        /// Gets the identifier of the user who performed the operation.
        /// </summary>
        public Guid? UserId { get; private set; }

        /// <summary>
        /// Gets the UTC timestamp when the operation occurred.
        /// </summary>
        public DateTime OccurredAtUtc { get; private set; }

        /// <summary>
        /// Gets the outcome of the operation (e.g., SUCCESS, FAILURE).
        /// </summary>
        public AuditOutcome Outcome { get; private set; }

        /// <summary>
        /// Gets the reason provided for the operation, if any.
        /// </summary>
        public string? Reason { get; private set; }

        /// <summary>
        /// Gets the previous state of the entity in JSON format.
        /// </summary>
        public string? BeforeState { get; private set; }

        /// <summary>
        /// Gets the new state of the entity in JSON format.
        /// </summary>
        public string? AfterState { get; private set; }

        /// <summary>
        /// Gets the list of fields that were changed during the operation in JSON format.
        /// </summary>
        public string? ChangedFields { get; private set; }

        /// <summary>
        /// Gets the metadata associated with this audit entry for event tracking.
        /// </summary>
        public EventMetadata? Metadata { get; private set; }

        /// <summary>
        /// Gets the correlation identifier for tracking related operations.
        /// </summary>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// Creates a new immutable audit log entry for a given operation.
        /// </summary>
        /// <param name="entityType">The type of entity affected.</param>
        /// <param name="entityId">The identifier of the affected entity.</param>
        /// <param name="operationType">The type of operation performed.</param>
        /// <param name="userId">The identifier of the user who performed the operation.</param>
        /// <param name="beforeState">The previous state of the entity (JSON format).</param>
        /// <param name="afterState">The new state of the entity (JSON format).</param>
        /// <param name="changedFields">The fields that were changed (JSON format).</param>
        /// <param name="outcome">The outcome of the operation.</param>
        /// <param name="reason">The reason provided for the operation.</param>
        /// <param name="relatedEntityId">Optional identifier of a related entity.</param>
        /// <param name="metadata">Optional metadata for the audit entry.</param>
        /// <param name="correlationId">Optional correlation ID for tracking related operations.</param>
        /// <returns>A new immutable AuditLog instance.</returns>
        public static AuditLog Create(
            AuditEntityType entityType,
            Guid entityId,
            AuditOperation operationType,
            Guid? userId,
            string? beforeState,
            string? afterState,
            string? changedFields,
            AuditOutcome outcome = AuditOutcome.Success,
            string? reason = null,
            Guid? relatedEntityId = null,
            EventMetadata? metadata = null,
            Guid? correlationId = null)
        {
            if (entityId == Guid.Empty)
            {
                throw new ArgumentException("Entity ID cannot be empty.", nameof(entityId));
            }

            return new AuditLog
            {
                Id = Guid.NewGuid(),
                EntityType = entityType,
                EntityId = entityId,
                RelatedEntityId = relatedEntityId,
                OperationType = operationType,
                UserId = userId,
                OccurredAtUtc = DateTime.UtcNow,
                BeforeState = beforeState,
                AfterState = afterState,
                ChangedFields = changedFields,
                Outcome = outcome,
                Reason = reason,
                Metadata = metadata ?? new EventMetadata(),
                CorrelationId = correlationId ?? Guid.NewGuid(),
            };
        }

        /// <summary>
        /// Validates the invariant that UPDATE operations must have both before and after states.
        /// </summary>
        /// <returns>True if the audit log entry is valid; otherwise, false.</returns>
        public bool IsValid()
        {
            // For UPDATE operations, both before and after states must be captured
            if (OperationType == AuditOperation.AccountUpdated ||
                OperationType == AuditOperation.UserUpdated)
            {
                return !string.IsNullOrEmpty(BeforeState) && !string.IsNullOrEmpty(AfterState);
            }

            // All operations must have a timestamp
            if (OccurredAtUtc == default)
            {
                return false;
            }

            // All operations must have user attribution
            if (!UserId.HasValue || UserId.Value == Guid.Empty)
            {
                return false;
            }

            return true;
        }
    }
}
