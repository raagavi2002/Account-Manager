// <copyright file="ApiErrorResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.ErrorResponses
{
        /// <summary>
        /// Standard error response model for API endpoints.
        /// </summary>
        public class ApiErrorResponse
        {
            /// <summary>
            /// Gets or sets the code.
            /// </summary>
            public string? Code { get; set; }

            /// <summary>
            /// Gets or sets the error message.
            /// </summary>
            public string Message { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the error details.
            /// </summary>
            public ApiErrorInfo? Details { get; set; }

            /// <summary>
            /// Gets or sets the correlation ID for request tracking.
            /// </summary>
            public string CorrelationId { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the timestamp when the error occurred.
            /// </summary>
            public string TimeStamp { get; set; } = string.Empty;
        }
}
