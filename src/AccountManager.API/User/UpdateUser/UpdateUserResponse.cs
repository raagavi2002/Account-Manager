// <copyright file="UpdateUserResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.UpdateUser
{
    /// <summary>
    /// Response DTO for the UpdateUser endpoint.
    /// Contains information about the updated user record.
    /// </summary>
    public class UpdateUserResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the updated user.
        /// </summary>
        /// <returns>A <see cref="Guid"/> representing the user ID.</returns>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the updated email address of the user.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the user's email.</returns>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the user record was last updated.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the update time in UTC.</returns>
        public DateTime UpdatedAt { get; set; }
    }
}
