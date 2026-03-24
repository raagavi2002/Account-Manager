// <copyright file="OpenSearchSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Shared.Configuration
{
    /// <summary>
    /// Represents configuration settings for OpenSearch logging.
    /// </summary>
    public class OpenSearchSettings
    {
        /// <summary>
        /// Gets or sets the URL of the OpenSearch node.
        /// </summary>
        required public string NodeUrl { get; set; }

        /// <summary>
        /// Gets or sets the username for OpenSearch authentication.
        /// </summary>
        required public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for OpenSearch authentication.
        /// </summary>
        required public string Password { get; set; }

        /// <summary>
        /// Gets or sets the index format for storing logs. Defaults to "logs-{0:yyyy.MM.dd}".
        /// </summary>
        required public string IndexFormat { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of events to post in a single batch. Defaults to 50.
        /// </summary>
        required public int BatchPostingLimit { get; set; }

        /// <summary>
        /// Gets or sets the period, in seconds, between batch postings. Defaults to 30.
        /// </summary>
        required public int Period { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled. Defaults to true.
        /// </summary>
        public bool EnableSsl { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether SSL certificates should be validated. Defaults to true.
        /// </summary>
        public bool ValidateCertificates { get; set; } = true;
    }
}
