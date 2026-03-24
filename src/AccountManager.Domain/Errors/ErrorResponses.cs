// <copyright file="ErrorResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Errors
{
    /// <summary>
    /// Represents a standardized error response returned by the system.
    /// </summary>
    public class ErrorResponses
    {
        /// <summary>
        /// Gets or sets the machine-readable error code.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the human-readable error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional details providing context about the error.
        /// </summary>
        public ErrorInfo? Details { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier used for request tracing.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp indicating when the error occurred.
        /// </summary>
        public string TimeStamp { get; set; } = string.Empty;
    }
}
