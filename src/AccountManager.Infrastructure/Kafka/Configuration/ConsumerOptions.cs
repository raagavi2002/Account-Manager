// <copyright file="ConsumerOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Kafka.Configuration
{
    /// <summary>
    /// Configuration options for Kafka consumers.
    /// </summary>
    public sealed class ConsumerOptions
    {
        /// <summary>
        /// Gets the consumer group ID used to identify the consumer group for Kafka.
        /// </summary>
        public string GroupId { get; init; } = default!;

        /// <summary>
        /// Gets the list of Kafka topics that this consumer subscribes to.
        /// </summary>
        public string[] Topics { get; init; } = Array.Empty<string>();
    }
}
