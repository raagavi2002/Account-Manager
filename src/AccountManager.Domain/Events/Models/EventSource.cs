// <copyright file="EventSource.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents the source information of a domain event.
    /// </summary>
    public class EventSource
    {
        /// <summary>
        /// Gets or sets the unique identifier of the service that produced the event.
        /// </summary>
        public string? ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the service that produced the event.
        /// </summary>
        public string? ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the service instance that produced the event.
        /// </summary>
        public string? InstanceId { get; set; }
    }
}
