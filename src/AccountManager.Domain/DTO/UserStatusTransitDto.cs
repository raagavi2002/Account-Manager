// <copyright file="UserStatusTransitDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    using AccountManager.Domain.Enums;

    /// <summary>
    /// Represents the data transfer object (DTO) used for transitioning a user's status.
    /// Contains the target status, reason, and version information for concurrency control.
    /// </summary>
    public class UserStatusTransitDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user whose status is being updated.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the target status for the user.
        /// Expected values are defined in the <see cref="UserStatus"/> enum (e.g., ACTIVE or INACTIVE).
        /// </summary>
        public UserStatus TargetStatus { get; set; }

        /// <summary>
        /// Gets or sets the business reason for the status change.
        /// This field is required for audit and compliance purposes.
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current version number of the user record.
        /// Used for optimistic locking to prevent concurrent update conflicts (SBR-ACM-006).
        /// </summary>
        public int Version { get; set; }
    }
}
