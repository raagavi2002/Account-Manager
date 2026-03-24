// <copyright file="ErrorDetails.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.Error
{
    /// <summary>
    /// Represents error details captured during event processing.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or sets the error type or exception name.
        /// </summary>
        required public string Type { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        required public string Message { get; set; }

        /// <summary>
        /// Gets or sets the stack trace associated with the error.
        /// </summary>
        required public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the number of retry attempts made.
        /// </summary>
        required public int RetryCount { get; set; }
    }
}
