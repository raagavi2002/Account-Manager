using System;

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents the payload for a user profile viewed event.
    /// </summary>
    public class UserProfileViewedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user whose profile was viewed.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account the user belongs to.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who viewed the profile.
        /// </summary>
        public Guid ViewedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the profile was viewed.
        /// </summary>
        public DateTime ViewedAt { get; set; }
    }
}
