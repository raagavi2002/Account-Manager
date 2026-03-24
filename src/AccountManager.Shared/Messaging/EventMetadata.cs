// <copyright file="EventMetadata.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Shared.Messaging
{
    /// <summary>
    /// Provides contextual metadata associated with an event,
    /// typically used for auditing, tracing, and multi-tenant support.
    /// </summary>
    public sealed class EventMetadata
    {
        /// <summary>
        /// Gets User who triggered the action (email or user id).
        /// </summary>
        public string? UserId { get; init; }

        /// <summary>
        /// Gets Role of the user (ADMIN, MAIN_CLIENT, etc.)
        /// </summary>
        public string? UserRole { get; init; }

        /// <summary>
        /// Gets Distributed tracing trace identifier.
        /// </summary>
        public string? TraceId { get; init; }

        /// <summary>
        /// Gets Distributed tracing span identifier.
        /// </summary>
        public string? SpanId { get; init; }

        /// <summary>
        /// Gets Tenant identifier for multi-tenant systems.
        /// </summary>
        public string? TenantId { get; init; }

        /// <summary>
        /// Gets Application name (e.g. Account Manager).
        /// </summary>
        public string? Application { get; init; }

        /// <summary>
        /// Gets Project or assembly name emitting the event.
        /// </summary>
        public string? Project { get; init; }

        /// <summary>
        /// Gets Domain class or aggregate name.
        /// </summary>
        public string? Class { get; init; }

        /// <summary>
        /// Gets Method or command that caused the event.
        /// </summary>
        public string? Method { get; init; }
    }
}
