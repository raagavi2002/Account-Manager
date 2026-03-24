// <copyright file="KafkaEventPublisher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Kafka.Producer
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Interfaces;
    using AccountManager.Infrastructure.Kafka.Configuration;
    using Confluent.Kafka;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Publishes domain events to Kafka topics using a string key and a JSON-serialized payload.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class encapsulates a Kafka producer instance which is thread-safe and reusable
    /// for multiple publish operations.
    /// </para>
    /// <para>
    /// It supports publishing both single messages and batches of messages to:
    /// <list type="bullet">
    /// <item>
    /// <description>The account events topic</description>
    /// </item>
    /// <item>
    /// <description>The dead-letter queue topic</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// The class implements <see cref="IDisposable"/> to ensure Kafka resources are
    /// properly flushed and released.
    /// </para>
    /// </remarks>
    public sealed class KafkaEventPublisher : IEventPublisher, IDisposable
    {
        private readonly IProducer<string, string> producer;
        private readonly string accountEventsTopic;
        private readonly string deadLetterTopic;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaEventPublisher"/> class.
        /// </summary>
        /// <param name="options">
        /// The Kafka configuration options containing bootstrap servers,
        /// topic names, and producer settings.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="options"/> is <c>null</c>.
        /// </exception>
        public KafkaEventPublisher(IOptions<KafkaOptions> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var config = new ProducerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                Acks = Acks.All,
            };

            producer = new ProducerBuilder<string, string>(config).Build();
            accountEventsTopic = options.Value.ProducerOptions.AccountEventsTopic;
            deadLetterTopic = options.Value.DeadLetterQueueOptions.DeadLetterTopic;
        }

        /// <summary>
        /// Publishes a single message to the account events Kafka topic.
        /// </summary>
        /// <param name="key">
        /// The message key used for Kafka partitioning.
        /// </param>
        /// <param name="payload">
        /// The JSON-serialized message payload.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous publish operation.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if the publisher has already been disposed.
        /// </exception>
        public async Task PublishToAccountEventTopicAsync(
            string key,
            string payload,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            await producer.ProduceAsync(
                accountEventsTopic,
                new Message<string, string>
                {
                    Key = key,
                    Value = payload,
                },
                cancellationToken);
        }

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
        public async Task PublishToAccountEventTopicAsync(IReadOnlyCollection<(string Key, string Payload)> messages, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            foreach (var (key, payload) in messages)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await producer.ProduceAsync(
                    accountEventsTopic,
                    new Message<string, string>
                    {
                        Key = key,
                        Value = payload,
                    },
                    cancellationToken);
            }
        }

        /// <summary>
        /// Publishes a single message to the dead-letter queue Kafka topic.
        /// </summary>
        /// <param name="key">
        /// The message key used for Kafka partitioning.
        /// </param>
        /// <param name="payload">
        /// The JSON-serialized message payload.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous publish operation.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if the publisher has already been disposed.
        /// </exception>
        public async Task PublishToDeadLetterQueueTopicAsync(string key, string payload, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            await producer.ProduceAsync(
                deadLetterTopic,
                new Message<string, string>
                {
                    Key = key,
                    Value = payload,
                },
                cancellationToken);
        }

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
        public async Task PublishToDeadLetterQueueTopicAsync(IReadOnlyCollection<(string Key, string Payload)> messages, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var deliveryTasks = new List<Task>();

            foreach (var (key, payload) in messages)
            {
                cancellationToken.ThrowIfCancellationRequested();

                deliveryTasks.Add(
                    producer.ProduceAsync(
                        deadLetterTopic,
                        new Message<string, string>
                        {
                            Key = key,
                            Value = payload,
                        },
                        cancellationToken));
            }

            // Await all deliveries
            await Task.WhenAll(deliveryTasks).ConfigureAwait(false);

            // Ensure everything is flushed to Kafka
            producer.Flush(cancellationToken);
        }

        /// <summary>
        /// Flushes any buffered Kafka messages and releases all managed resources.
        /// </summary>
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            producer.Flush(TimeSpan.FromSeconds(10));
            producer.Dispose();
            disposed = true;
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the publisher
        /// has already been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Thrown when the publisher is disposed.
        /// </exception>
        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(KafkaEventPublisher));
            }
        }
    }
}
