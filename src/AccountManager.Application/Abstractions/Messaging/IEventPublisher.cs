// <copyright file="IEventPublisher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Interfaces
{
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Defines an abstraction for publishing domain events
    /// to an external messaging or eventing infrastructure.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes a raw message to a specified Kafka topic.
        /// Used by the outbox worker to replay stored messages.
        /// </summary>
        /// <param name="key">The message key for partitioning.</param>
        /// <param name="payload">The serialized JSON payload.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the async operation.</returns>
        Task PublishToAccountEventTopicAsync(
            string key,
            string payload,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Publishes a raw message to a specified Kafka topic.
        /// Used by the outbox worker to replay stored messages.
        /// </summary>
        /// <param name="key">The message key for partitioning.</param>
        /// <param name="payload">The serialized JSON payload.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the async operation.</returns>
        Task PublishToDeadLetterQueueTopicAsync(
            string key,
            string payload,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Publishes multiple messages to the dead-letter queue Kafka topic.
        /// </summary>
        /// <param name="messages">
        /// A collection of messages where each item contains a message key
        /// and a JSON-serialized payload.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while publishing the messages.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous batch publish operation.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if the publisher has already been disposed.
        /// </exception>
        Task PublishToDeadLetterQueueTopicAsync(IReadOnlyCollection<(string Key, string Payload)> messages, CancellationToken cancellationToken = default);

        /// <summary>
        /// Publishes multiple messages to the account events Kafka topic.
        /// </summary>
        /// <param name="messages">
        /// A collection of messages where each item contains a message key
        /// and a JSON-serialized payload.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while publishing the messages.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous batch publish operation.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if the publisher has already been disposed.
        /// </exception>
        /// <remarks>
        /// Messages are produced sequentially to preserve ordering within
        /// the scope of this call.
        /// </remarks>
        Task PublishToAccountEventTopicAsync(IReadOnlyCollection<(string Key, string Payload)> messages, CancellationToken cancellationToken = default);
    }
}
