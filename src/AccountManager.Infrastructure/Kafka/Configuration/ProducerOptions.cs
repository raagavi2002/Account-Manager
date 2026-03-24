// <copyright file="ProducerOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Kafka.Configuration
{
    /// <summary>
    /// Configuration options for Kafka producers.
    /// </summary>
    public sealed class ProducerOptions
    {
        /// <summary>
        /// Gets the name of the Kafka topic used for account-related events.
        /// </summary>
        public string AccountEventsTopic { get; init; } = default!;
    }
}
