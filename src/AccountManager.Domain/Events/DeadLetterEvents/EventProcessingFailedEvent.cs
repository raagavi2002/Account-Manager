// <copyright file="EventProcessingFailedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.DeadLetterEvents
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data.Error;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when event processing fails.
    /// </summary>
    public class EventProcessingFailedEvent
        : BaseEvent<EventProcessingFailedData>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EventProcessingFailedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public EventProcessingFailedEvent()
        {
            this.EventType = EventTypes.EventProcessingFailed;
        }
    }
}
