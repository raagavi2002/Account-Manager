// <copyright file="GetUserResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.GetUser
{
    /// <summary>
    /// Response DTO for the GetUser endpoint.
    /// Contains user profile information including roles and permissions.
    /// </summary>
    public class GetUserResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <returns>A <see cref="Guid"/> representing the user ID.</returns>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the account the user belongs to.
        /// </summary>
        /// <returns>A <see cref="Guid"/> representing the account ID.</returns>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the user's email.</returns>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the user's first name.</returns>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the user's last name.</returns>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the list of roles assigned to the user.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/> values representing roles.</returns>
        public List<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets the permissions assigned to the user.
        /// </summary>
        /// <returns>An <see cref="object"/> representing the user's permissions. 
        /// Replace with a strongly typed PermissionSet when available.</returns>
        public object Permissions { get; set; } // Replace with actual PermissionSet type

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        /// <returns><c>true</c> if the user is active; otherwise, <c>false</c>.</returns>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the user's last login.
        /// </summary>
        /// <returns>A nullable <see cref="DateTime"/> representing the last login time.</returns>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Gets or sets the total number of times the user has logged in.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the login count.</returns>
        public int LoginCount { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the user account was created.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the account creation time.</returns>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the version number of the user record.
        /// Useful for concurrency checks and optimistic locking.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the record version.</returns>
        public int Version { get; set; }
    }
}
