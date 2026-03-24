// <copyright file="IEventFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Events
{
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Defines a factory for creating fully populated
    /// domain event instances.
    /// </summary>
    public interface IEventFactory
    {
        /// <summary>
        /// Creates a new domain event instance and populates
        /// standard event metadata such as source and timestamps.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to create.</typeparam>
        /// <typeparam name="TData">The type of the event payload.</typeparam>
        /// <param name="data">The event payload.</param>
        /// <param name="metadata">
        /// Optional metadata describing the execution context.
        /// </param>
        /// <returns>A fully initialized domain event.</returns>
        public TEvent CreateEvent<TEvent, TData>(
           TData data,
           EventMetadata metadata)
           where TEvent : BaseEvent<TData>, new()
           where TData : class;
    }
}
