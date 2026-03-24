// <copyright file="BaseEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;

    /// <summary>
    /// Represents the base type for all domain events.
    /// </summary>
    /// <typeparam name="TData">
    /// The type of the event payload data.
    /// </typeparam>
    public abstract class BaseEvent<TData>
        where TData : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEvent{TData}"/> class
        /// with default values for identifiers, timestamp, and event version.
        /// </summary>
        [SetsRequiredMembers]
        protected BaseEvent()
        {
            this.EventId = Guid.NewGuid();
            this.Timestamp = DateTime.UtcNow;
            this.CorrelationId = Guid.NewGuid();
            this.EventVersion = EventVersions.V1;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        required public Guid EventId { get; set; }

        /// <summary>
        /// Gets or sets the logical type of the event.
        /// </summary>
        public string? EventType { get; set; }

        /// <summary>
        /// Gets or sets the version of the event contract.
        /// </summary>
        required public string EventVersion { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp indicating when the event occurred.
        /// </summary>
        required public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier used to associate
        /// related events across systems.
        /// </summary>
        required public Guid CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the source information describing the service
        /// that produced the event.
        /// </summary>
        public EventSource? Source { get; set; }

        /// <summary>
        /// Gets or sets additional metadata providing contextual and
        /// diagnostic information about the event.
        /// </summary>
        public EventMetadata? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the event payload data.
        /// </summary>
        public TData? Data { get; set; }
    }
}
