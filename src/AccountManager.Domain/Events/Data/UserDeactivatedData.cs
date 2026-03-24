// <copyright file="UserDeactivatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents the payload for a user deactivated event.
    /// </summary>
    public class UserDeactivatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        required public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account the user belongs to.
        /// </summary>
        required public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        required public string Email { get; set; }

        /// <summary>
        /// Gets or sets the reason for the user deactivation.
        /// </summary>
        required public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the user was deactivated.
        /// </summary>
        required public DateTime DeactivatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that deactivated the user.
        /// </summary>
        required public string DeactivatedBy { get; set; }
    }
}
