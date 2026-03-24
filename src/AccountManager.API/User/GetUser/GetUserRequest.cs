// <copyright file="GetUserRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.GetUser
{
    /// <summary>
    /// Represents the request model for retrieving a user profile.
    /// </summary>
    public class GetUserRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to be retrieved.
        /// </summary>
        /// <returns>
        /// A <see cref="Guid"/> representing the user ID.
        /// </returns>
        public Guid UserId { get; set; }
    }
}
