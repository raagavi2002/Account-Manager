// <copyright file="AddUserDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    using AccountManager.Domain.Enums;

    /// <summary>
    /// Data Transfer Object used to add a new user to an account.
    /// Contains identifying information, contact details, and role assignments.
    /// </summary>
    public class AddUserDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account to which the user belongs.
        /// </summary>
        required public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user. This field is optional.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the type of user, such as Admin or Standard.
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// Gets or sets the list of roles assigned to the user.
        /// Defaults to an empty list.
        /// </summary>
        public List<UserRoleType> Roles { get; set; } = new List<UserRoleType>();
    }
}
