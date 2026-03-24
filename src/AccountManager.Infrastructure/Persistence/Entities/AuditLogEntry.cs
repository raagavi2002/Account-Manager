// <copyright file="AuditLogEntry.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents an entry in the audit log, capturing details of operations performed
/// on entities within the system for accountability and traceability.
/// </summary>
[Table("audit_log_entries", Schema = "am")]
public partial class AuditLogEntry
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit log entry.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the type of operation performed (e.g., create, update, delete).
    /// </summary>
    [Column("operation")]
    public int Operation { get; set; }

    /// <summary>
    /// Gets or sets the type of entity involved in the operation.
    /// </summary>
    [Column("entity_type")]
    public int EntityType { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the entity affected by the operation.
    /// </summary>
    [Column("entity_id")]
    public Guid EntityId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of a related entity, if applicable.
    /// </summary>
    [Column("related_entity_id")]
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who performed the operation.
    /// </summary>
    [Column("actor_user_id")]
    public Guid? ActorUserId { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the operation occurred.
    /// </summary>
    [Column("occurred_at_utc")]
    public DateTime OccurredAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the outcome of the operation (e.g., success, failure).
    /// </summary>
    [Column("outcome")]
    public int Outcome { get; set; }

    /// <summary>
    /// Gets or sets the audit status of the entry.
    /// </summary>
    [Column("audit_status")]
    public int AuditStatus { get; set; }

    /// <summary>
    /// Gets or sets the reason provided for the operation, if any.
    /// </summary>
    [Column("reason")]
    public string? Reason { get; set; }

    /// <summary>
    /// Gets or sets the previous state of the entity in JSON format.
    /// </summary>
    [Column("previous_state", TypeName = "jsonb")]
    public string? PreviousState { get; set; }

    /// <summary>
    /// Gets or sets the new state of the entity in JSON format.
    /// </summary>
    [Column("new_state", TypeName = "jsonb")]
    public string? NewState { get; set; }

    /// <summary>
    /// Gets or sets the fields that were changed during the operation in JSON format.
    /// </summary>
    [Column("changed_fields", TypeName = "jsonb")]
    public string? ChangedFields { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation was related to a GDPR request.
    /// </summary>
    [Column("is_gdpr_request")]
    public bool IsGdprRequest { get; set; }

    /// <summary>
    /// Gets or sets the correlation identifier used to group related audit log entries.
    /// </summary>
    [Column("correlation_id")]
    public Guid? CorrelationId { get; set; }
}
