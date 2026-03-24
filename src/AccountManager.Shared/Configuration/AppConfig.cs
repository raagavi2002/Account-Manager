// <copyright file="AppConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Shared.Configuration
{
    /// <summary>
    /// Represents application configuration settings.
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the service.
        /// </summary>
        public string ServiceVersion { get; set; } = string.Empty;
    }
}
