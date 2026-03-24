// <copyright file="KafkaOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Kafka.Configuration
{
    /// <summary>
    /// Configuration options for connecting to and interacting with a Kafka cluster.
    /// </summary>
    public sealed class KafkaOptions
    {
        /// <summary>
        /// Gets the Kafka bootstrap servers in the format "host1:port1,host2:port2".
        /// </summary>
        public string BootstrapServers { get; init; } = default!;

        /// <summary>
        /// Gets the security-related configuration options for Kafka connections.
        /// </summary>
        required public KafkaSecurityOptions KafkaSecurityOptions { get; init; }

        /// <summary>
        /// Gets the configuration options for Kafka producers.
        /// </summary>
        required public ProducerOptions ProducerOptions { get; init; }

        /// <summary>
        /// Gets the configuration options for Kafka consumers.
        /// </summary>
        required public ConsumerOptions ConsumerOptions { get; init; }

        /// <summary>
        /// Gets the configuration options for the dead-letter queue.
        /// </summary>
        required public DeadLetterQueueOptions DeadLetterQueueOptions { get; init; }
    }
}
