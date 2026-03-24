// <copyright file="KafkaProducedEventDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Reprsenets a Kafka produced event for data transfer.
    /// </summary>
    public class KafkaProducedEventDto
    {
        /// <summary>
        /// Gets or sets the Primary key for the produced event.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Account Id associated with the event.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the kafka event topic name.
        /// </summary>
        public string? TopicName { get; set; }

        /// <summary>
        /// Gets or sets the Logical event type or topic name.
        /// </summary>
        public string? EventType { get; set; }

        /// <summary>
        /// Gets or sets the Service that produced the event.
        /// </summary>
        public string? ProducerService { get; set; }

        /// <summary>
        /// Gets or sets the Event payload serialized as JSON.
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// Gets or sets the When the event was produced.
        /// </summary>
        public DateTime ProducedAt { get; set; }

        /// <summary>
        /// Gets or sets the Optional correlation id for distributed tracing.
        /// </summary>
        public Guid? CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the Processing or send status (for example, "SENT").
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the Optional error message captured when producing failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the Number of retry attempts.
        /// </summary>
        public int? RetryCount { get; set; }
    }
}
