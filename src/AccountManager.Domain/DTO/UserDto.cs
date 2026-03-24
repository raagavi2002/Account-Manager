// <copyright file="UserDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Represents a lightweight data transfer object (DTO) for user information.
    /// This class is used to transfer user data between application layers
    /// without exposing persistence-specific details.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user last logged in.
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Gets or sets the total number of times the user has logged in.
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// Gets or sets the roles assigned to the user.
        /// </summary>
        public List<string> Roles { get; set; } = new ();

        /// <summary>
        /// Gets or sets the version number for concurrency control.
        /// </summary>
        public int? Version { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the account to which the user belongs.
        /// </summary>
        public Guid AccountId { get; set; }
    }
}
