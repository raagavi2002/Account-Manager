// <copyright file="DeadLetterQueueOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Kafka.Configuration
{
    /// <summary>
    /// Represents configuration options for the Kafka dead-letter queue.
    /// </summary>
    public class DeadLetterQueueOptions
    {
        /// <summary>
        /// Gets the name of the Kafka topic used as the dead-letter queue.
        /// </summary>
        required public string DeadLetterTopic { get; init; }
    }
}
