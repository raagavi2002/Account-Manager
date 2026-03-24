// <copyright file="LogMetadata.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Shared.Logging
{
    /// <summary>
    /// Represents metadata information for logging purposes.
    /// </summary>
    public class LogMetaData
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        required public string ServiceName { get; init; }

        /// <summary>
        /// Gets the version of the service.
        /// </summary>
        required public string ServiceVersion { get; init; }

        /// <summary>
        /// Gets the environment where the service is running (e.g., Development, Production).
        /// </summary>
        required public string Environment { get; init; }

        /// <summary>
        /// Gets the project layer (e.g., Application, Domain, Infrastructure).
        /// </summary>
        required public string ProjectLayer { get; init; }

        /// <summary>
        /// Gets the name of the class generating the log.
        /// </summary>
        required public string ClassName { get; init; }

        /// <summary>
        /// Gets the name of the method generating the log.
        /// </summary>
        required public string MethodName { get; init; }

        /// <summary>
        /// Gets the timestamp when the log entry was created.
        /// </summary>
        public DateTimeOffset Timestamp { get; init; }

        /// <summary>
        /// Gets the optional correlation ID for tracing requests across systems.
        /// </summary>
        public string? CorrelationId { get; init; }
    }
}
