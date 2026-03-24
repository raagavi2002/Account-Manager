// <copyright file="ErrorInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Errors
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents contextual information associated with an error.
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// Gets or sets the product identifier associated with the error.
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Gets or sets the account identifier associated with the error.
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Gets or sets additional key-value data that provides context for the error.
        /// </summary>
        public Dictionary<string, string>? AdditionalInfo { get; set; }
    }
}
