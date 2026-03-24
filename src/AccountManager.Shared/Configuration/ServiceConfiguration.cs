// <copyright file="ServiceConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Shared.Configuration
{
    /// <summary>
    /// Configuration options for service identification in events.
    /// </summary>
    public sealed class ServiceConfiguration
    {
        /// <summary>
        /// Gets the unique identifier for this service (e.g., "account-manager").
        /// </summary>
        public string ServiceId { get; init; } = default!;

        /// <summary>
        /// Gets the human-readable name of this service (e.g., "Account Manager").
        /// </summary>
        public string ServiceName { get; init; } = default!;

        /// <summary>
        /// Gets the specific instance/pod identifier for this service instance.
        /// </summary>
        public string InstanceId { get; init; } = default!;
    }
}
