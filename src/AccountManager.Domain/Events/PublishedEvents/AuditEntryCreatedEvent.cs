// <copyright file="AuditEntryCreatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.PublishedEvents
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when a new audit log entry is created.
    /// </summary>
    public class AuditEntryCreatedEvent : BaseEvent<AuditEntryCreatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditEntryCreatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public AuditEntryCreatedEvent()
        {
            // Initialize required properties with default values
            EventType = EventTypes.AuditEntryCreated;
        }
    }
}
