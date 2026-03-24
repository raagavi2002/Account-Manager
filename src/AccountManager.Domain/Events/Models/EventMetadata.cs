// <copyright file="EventMetadata.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents metadata associated with a domain event, providing contextual
    /// and diagnostic information for tracing, auditing, and authorization.
    /// </summary>
    public class EventMetadata
    {
        /// <summary>
        /// Gets or sets the identifier of the user who initiated the action
        /// that resulted in the event.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the role of the user at the time the event was generated.
        /// </summary>
        public string? UserRole { get; set; }

        /// <summary>
        /// Gets or sets the distributed trace identifier for correlating events
        /// across services.
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// Gets or sets the span identifier within the distributed trace.
        /// </summary>
        public string? SpanId { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier associated with the event.
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name of the application that generated the event.
        /// </summary>
        public string? Application { get; set; }

        /// <summary>
        /// Gets or sets the project or bounded context within the application
        /// where the event originated.
        /// </summary>
        public string? Project { get; set; }

        /// <summary>
        /// Gets or sets the name of the class in which the event was generated.
        /// </summary>
        public string? Class { get; set; }

        /// <summary>
        /// Gets or sets the name of the method in which the event was generated.
        /// </summary>
        public string? Method { get; set; }
    }
}
