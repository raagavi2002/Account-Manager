// <copyright file="UserCreatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AccountManager.Domain.Enums;

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents the payload for a user created event.
    /// </summary>
    public class UserCreatedData
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
        /// Gets or sets the user's first name.
        /// </summary>
        required public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        required public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the roles assigned to the user.
        /// </summary>
        required public List<UserRoleType> Roles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        required public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the external identity provider user identifier.
        /// </summary>
        required public string ClerkUserId { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the user was created.
        /// </summary>
        required public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that created the user.
        /// </summary>
        required public string CreatedBy { get; set; }
    }
}
