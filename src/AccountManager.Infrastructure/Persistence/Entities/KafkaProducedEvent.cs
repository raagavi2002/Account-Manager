// <copyright file="KafkaProducedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents an event that has been produced to Kafka for event-driven communication.
/// Tracks the status and metadata of outbound Kafka messages for audit and retry purposes.
/// </summary>
[Table("kafka_produced_events", Schema = "am")]
[Index("CorrelationId", Name = "idx_produced_correlation")]
[Index("EventType", "ProducedAt", Name = "idx_produced_topic_time")]
public partial class KafkaProducedEvent
{
    /// <summary>
    /// Gets or sets the unique identifier for the Kafka produced event.
    /// </summary>
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the event being produced (e.g., AccountCreated, UserUpdated).
    /// </summary>
    [Column("event_type")]
    [StringLength(255)]
    public string? EventType { get; set; }

    /// <summary>
    /// Gets or sets the name of the service that produced this event.
    /// </summary>
    [Column("producer_service")]
    [StringLength(255)]
    public string? ProducerService { get; set; }

    /// <summary>
    /// Gets or sets the JSON payload of the event message.
    /// </summary>
    [Column("payload", TypeName = "jsonb")]
    public string? Payload { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the event was produced.
    /// </summary>
    [Column("produced_at")]
    public DateTime ProducedAt { get; set; }

    /// <summary>
    /// Gets or sets the correlation identifier for tracking related events across services.
    /// </summary>
    [Column("correlation_id")]
    public Guid? CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the current status of the event production (e.g., PENDING, SUCCESS, FAILED).
    /// </summary>
    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the error message if the event production failed (optional).
    /// </summary>
    [Column("error_message")]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the number of retry attempts for failed event production (optional).
    /// </summary>
    [Column("retry_count")]
    public int? RetryCount { get; set; }

    /// <summary>
    /// Gets or sets the name of the Kafka topic to which the event was produced.
    /// </summary>
    [Column("topic_name")]
    public string? TopicName { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the account associated with this event (optional).
    /// </summary>
    [Column("account_id")]
    public Guid? AccountId { get; set; }
}
