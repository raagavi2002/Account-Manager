// <copyright file="EventFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Events
{
    using AccountManager.Application.Events;
    using AccountManager.Domain.Events.Models;
    using AccountManager.Shared.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Default implementation of <see cref="IEventFactory"/>
    /// responsible for creating domain events using
    /// runtime configuration.
    /// </summary>
    public sealed class EventFactory : IEventFactory
    {
        private readonly ServiceConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFactory"/> class.
        /// </summary>
        /// <param name="config">
        /// Configuration containing service identity and event settings.
        /// </param>
        public EventFactory(IOptions<ServiceConfiguration> config)
        {
            this.config = config.Value;
        }

        /// <summary>
        /// Creates a new domain event instance of type <typeparamref name="TEvent"/> with the specified event data and metadata.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to create. Must inherit from <see cref="BaseEvent{TData}"/> and have a parameterless constructor.</typeparam>
        /// <typeparam name="TData">The type of the event data.</typeparam>
        /// <param name="data">The event data to include in the event.</param>
        /// <param name="metadata">The metadata associated with the event.</param>
        /// <returns>A new instance of <typeparamref name="TEvent"/> populated with the provided data and metadata.</returns>
        public TEvent CreateEvent<TEvent, TData>(
            TData data,
            EventMetadata metadata)
            where TEvent : BaseEvent<TData>, new()
            where TData : class
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(metadata);
            var eventInstance = new TEvent
            {
                Source = new EventSource
                {
                    ServiceId = config.ServiceId,
                    ServiceName = config.ServiceName,
                    InstanceId = config.InstanceId,
                },
                Metadata = metadata,
                Data = data,
            };

            return eventInstance;
        }
    }
}
