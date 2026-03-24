// <copyright file="EventProcessingFailedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.Error
{
    /// <summary>
    /// Represents the payload for an event processing failure.
    /// </summary>
    public class EventProcessingFailedData
    {
        /// <summary>
        /// Gets or sets the original event that failed processing.
        /// </summary>
        required public object OriginalEvent { get; set; }

        /// <summary>
        /// Gets or sets the error details describing the failure.
        /// </summary>
        required public ErrorDetails Error { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when processing failed.
        /// </summary>
        public DateTime FailedAt { get; set; }
    }
}
