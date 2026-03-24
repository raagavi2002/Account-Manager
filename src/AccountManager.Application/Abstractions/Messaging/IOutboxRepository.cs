// <copyright file="IOutboxRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Abstractions.Messaging
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Domain.DTO;

    /// <summary>
    /// Defines a repository contract for managing Kafka-produced events
    /// using the outbox pattern.
    /// </summary>
    public interface IOutboxRepository
    {
        /// <summary>
        /// Retrieves a batch of pending Kafka-produced events that are ready for processing.
        /// </summary>
        /// <param name="maxEvents">The maximum number of events to retrieve.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>
        /// A read-only list of pending <see cref="KafkaProducedEventDto"/> instances.
        /// </returns>
        Task<IReadOnlyList<KafkaProducedEventDto>> GetPendingKafkaProducedEvents(
            int maxEvents,
            CancellationToken cancellationToken);

        /// <summary>
        /// Marks the specified Kafka-produced events as successfully processed.
        /// </summary>
        /// <param name="eventIds">The identifiers of the events to mark as processed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>representing the asynchronous operation.</returns>
        Task MarkAsProcessedAsync(
            List<long> eventIds,
            CancellationToken cancellationToken);

        /// <summary>
        /// Marks the specified Kafka-produced events as dead-lettered in a batch operation.
        /// </summary>
        /// <param name="eventIds">The identifiers of the events to mark as dead-lettered.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>representing the asynchronous operation.</returns>
        Task MarkAsDeadLetterBatchAsync(
            List<long> eventIds,
            CancellationToken cancellationToken);

        /// <summary>
        /// Increments the retry count for the specified Kafka-produced event.
        /// </summary>
        /// <param name="eventId">The identifier of the event whose retry count should be incremented.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>representing the asynchronous operation.</returns>
        Task IncrementRetryAsync(
            long eventId,
            CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new Kafka-produced event to the outbox for later processing.
        /// </summary>
        /// <param name="kafkaProducedEvent">The Kafka-produced event data to persist.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>representing the asynchronous operation.</returns>
        Task AddKafkaProducedEventAsync(
            KafkaProducedEventDto kafkaProducedEvent,
            CancellationToken cancellationToken);
    }
}
