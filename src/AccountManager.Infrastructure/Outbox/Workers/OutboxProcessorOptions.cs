// <copyright file="OutboxProcessorOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Outbox.Workers
{
    /// <summary>
    /// Configuration options for the Kafka outbox processor.
    /// <para>
    /// These options control batching behavior, retry limits, polling frequency,
    /// and parallel processing for the Kafka outbox background worker.
    /// </para>
    /// </summary>
    public class OutboxProcessorOptions
    {
        /// <summary>
        /// Gets or sets the maximum number of outbox messages
        /// to retrieve and process in a single batch.
        /// </summary>
        /// <remarks>
        /// Larger batch sizes increase throughput but may increase memory usage
        /// and processing latency.
        /// </remarks>
        public int BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of retry attempts allowed
        /// for an outbox message before it is moved to the dead-letter queue.
        /// </summary>
        /// <remarks>
        /// Once this threshold is reached, the message is considered permanently
        /// failed and will no longer be retried.
        /// </remarks>
        public int MaxRetries { get; set; }

        /// <summary>
        /// Gets or sets the polling interval, in seconds, used by the outbox worker
        /// to check for new pending messages.
        /// </summary>
        /// <remarks>
        /// Lower values reduce latency but increase database load.
        /// Higher values reduce load but may delay event publication.
        /// </remarks>
        public int PollingIntervalSeconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of messages
        /// that can be processed concurrently.
        /// </summary>
        /// <remarks>
        /// This value controls parallelism and should be tuned based on
        /// Kafka throughput, database capacity, and available system resources.
        /// </remarks>
        public int MaxParallelism { get; set; }
    }
}
