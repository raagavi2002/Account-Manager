// <copyright file="UserStatusTransitEndpointResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.User_Status_Transit
{
    /// <summary>
    /// Represents the response payload for the user status transition endpoint.
    /// </summary>
    public class UserStatusTransitEndpointResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user whose status was updated.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is now active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the status change occurred.
        /// </summary>
        public DateTime StatusChangedAt { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who performed the status change.
        /// </summary>
        public Guid StatusChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the new version number after the status update.
        /// Used for concurrency and tracking changes.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the recorded reason for the status change.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}
