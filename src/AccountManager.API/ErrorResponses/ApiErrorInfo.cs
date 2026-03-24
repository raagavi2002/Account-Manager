// <copyright file="ApiErrorInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.ErrorResponses
{
    /// <summary>
    /// Additional details about the error.
    /// </summary>
    public class ApiErrorInfo
    {
        /// <summary>
        /// Gets or sets the product ID associated with the error.
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Gets or sets the account ID associated with the error.
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Gets or sets additional context information.
        /// </summary>
        public Dictionary<string, string>? AdditionalInfo { get; set; }
    }
}
