// <copyright file="EventEnvelope.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Shared.Messaging
{
    using System.Diagnostics.Tracing;

    /// <summary>
    /// Represents a standardized envelope for events exchanged through the messaging system.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the event payload.
    /// </typeparam>
    public sealed class EventEnvelope<T>
    {
        /// <summary>
        /// Gets the unique identifier of the event.
        /// </summary>
        public Guid EventId { get; init; }

        /// <summary>
        /// Gets the logical type of the event (e.g., OrderCreated).
        /// </summary>
        public string EventType { get; init; } = default!;

        /// <summary>
        /// Gets the version of the event schema.
        /// Defaults to <c>1.0</c>.
        /// </summary>
        public string EventVersion { get; init; } = "1.0";

        /// <summary>
        /// Gets the UTC timestamp indicating when the event occurred.
        /// </summary>
        public DateTime Timestamp { get; init; }

        /// <summary>
        /// Gets the identifier used to correlate related events across services.
        /// </summary>
        public Guid CorrelationId { get; init; }

        /// <summary>
        /// Gets the source that produced the event.
        /// </summary>
        public EventSource Source { get; init; } = default!;

        /// <summary>
        /// Gets optional metadata providing additional contextual information about the event.
        /// </summary>
        public EventMetadata? Metadata { get; init; }

        /// <summary>
        /// Gets the event payload.
        /// </summary>
        public T Data { get; init; } = default!;
    }
}
