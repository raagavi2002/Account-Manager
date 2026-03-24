// <copyright file="KafkaSecurityOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Kafka.Configuration
{
    /// <summary>
    /// Represents security-related configuration options for connecting to a Kafka cluster.
    /// </summary>
    public sealed class KafkaSecurityOptions
    {
        /// <summary>
        /// Gets the username used for authentication with the Kafka broker.
        /// </summary>
        required public string Username { get; init; }

        /// <summary>
        /// Gets the password used for authentication with the Kafka broker.
        /// </summary>
        required public string Password { get; init; }

        /// <summary>
        /// Gets the SASL mechanism used for authentication.
        /// Defaults to <c>SCRAM-SHA-512</c>.
        /// </summary>
        required public string Mechanism { get; init; }
    }
}
