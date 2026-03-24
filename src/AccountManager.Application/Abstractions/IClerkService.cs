// <copyright file="IClerkService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Abstractions
{
    /// <summary>
    /// Interface for interacting with Clerk authentication service.
    /// </summary>
    public interface IClerkService
    {
        /// <summary>
        /// Creates a new user in Clerk.
        /// </summary>
        /// <param name="user">The user data to create.</param>
        /// <param name="orgId">The organization ID (optional, uses config default).</param>
        /// <param name="role">The role in the organization (optional, uses config default).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Clerk user ID.</returns>
        Task<string> CreateUserAsync(AdUser user, string? orgId = null, string? role = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a user to an organization in Clerk.
        /// </summary>
        /// <param name="userId">The Clerk user ID.</param>
        /// <param name="orgId">The organization ID (optional, uses config default).</param>
        /// <param name="role">The role in the organization (optional, uses config default).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddUserToOrganizationAsync(string userId, string? orgId = null, string? role = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifies if the Clerk API is working.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if API is working, false otherwise.</returns>
        Task<bool> VerifyApiAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a user for Clerk operations.
    /// </summary>
    public class AdUser
    {
        /// <summary>
        /// Gets or sets the object GUID.
        /// </summary>
        public Guid ObjectGuid { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public List<string> Roles { get; set; } = new();
    }
}