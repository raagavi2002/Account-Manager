// <copyright file="AddUserResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Results
{
    using AccountManager.Domain.Enums;

    /// <summary>
    /// Represents the result of adding a new user to the system, including user details and metadata.
    /// </summary>
    public class AddUserResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the newly created user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the roles assigned to the user.
        /// </summary>
        public List<UserRoleType> Roles { get; set; } = new List<UserRoleType>();

        /// <summary>
        /// Gets or sets the user identifier from the Clerk authentication system.
        /// </summary>
        public string ClerkUserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        /// <remarks>
        /// The initial state of a newly created user is <c>true</c>.
        /// </remarks>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
