// <copyright file="UpdateUserRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.UpdateUser
{
    /// <summary>
    /// Request DTO for the UpdateUser endpoint.
    /// Contains the information required to update a user's profile.
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to be updated.
        /// </summary>
        /// <returns>A <see cref="Guid"/> representing the user ID.</returns>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the new first name of the user.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the user's first name.</returns>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the new last name of the user.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the user's last name.</returns>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the new email address of the user.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the user's email address.</returns>
        public string Email { get; set; }
    }
}
