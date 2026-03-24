// <copyright file="UserActivatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents the payload for a user activated event.
    /// </summary>
    public class UserActivatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account the user belongs to.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        required public string Email { get; set; }

        /// <summary>
        /// Gets or sets the reason for the user activation.
        /// </summary>
        required public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the user was activated.
        /// </summary>
        required public DateTime ActivatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that activated the user.
        /// </summary>
        required public string ActivatedBy { get; set; }
    }
}
