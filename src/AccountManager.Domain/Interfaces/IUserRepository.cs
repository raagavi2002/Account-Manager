// <copyright file="IUserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Interfaces
{
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Results;

    /// <summary>
    /// Defines a contract for user data persistence operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Checks whether an account has at least one user with the Main Client role.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains <c>true</c> if a Main Client user exists; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> HasMainClientUserAsync(Guid accountId);

        /// <summary>
        /// Adds a new user to the specified account.
        /// </summary>
        /// <param name="userDto">The data transfer object containing user information.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains an <see cref="AddUserResult"/> with details of the added user.
        /// </returns>
        Task<AddUserResult> AddUserAsync(AddUserDto userDto);

        /// <summary>
        /// Updates the Clerk user ID for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="clerkUserId">The Clerk user ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateClerkUserIdAsync(Guid userId, string clerkUserId);

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user to retrieve.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains
        /// the <see cref="User"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the status of a user by their unique identifier.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user whose status should be updated.
        /// </param>
        /// <param name="status">
        /// The new status value as a string. Expected values are "ACTIVE" or "INACTIVE".
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result is <c>true</c> if the update succeeded,
        /// or <c>false</c> if the user was not found or the status value was invalid.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided status string is not recognized.
        /// </exception>
        Task<bool> UpdateUserStatusAsync(Guid userId, string status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether a user is active and assigned to the specified role.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user.
        /// </param>
        /// <param name="roleType">
        /// The role type to verify against the user's assigned roles.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// <c>true</c> if the user is active and has the specified role; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsUserInRoleAsync(
            Guid userId,
            UserRoleType roleType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether a user with the specified email address already exists in the system.
        /// The comparison is case-insensitive to prevent near-duplicate entries.
        /// </summary>
        /// <param name="email">The email address to check for existence.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> that returns <c>true</c> if the email already exists;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="email"/> is null or empty.</exception>
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether a user with the specified first name and last name combination already exists.
        /// The comparison is case-insensitive. An optional <paramref name="excludeUserId"/> can be
        /// provided to exclude a specific user from the check, which is useful during update operations.
        /// </summary>
        /// <param name="firstName">The first name to check for duplication.</param>
        /// <param name="lastName">The last name to check for duplication.</param>
        /// <param name="excludeUserId">
        /// An optional user ID to exclude from the duplicate check. Pass the current user's ID
        /// when validating an update to avoid false positives against the same record.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> that returns <c>true</c> if a duplicate name combination
        /// exists; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="firstName"/> or <paramref name="lastName"/> is null or empty.
        /// </exception>
        Task<bool> DuplicateNameExistsAsync(
            string firstName,
            string lastName,
            Guid? excludeUserId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves users associated with an account with optional filtering and pagination.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="isActive">Optional filter for active/inactive users.</param>
        /// <param name="role">Optional filter for user role.</param>
        /// <param name="pageSize">The number of results per page (max 100).</param>
        /// <param name="pageNumber">The page number starting from 1.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a tuple with
        /// the list of users and the total count of matching users.
        /// </returns>
        Task<(List<UserDto> Users, int TotalCount)> GetAccountUsersAsync(
            Guid accountId,
            bool? isActive = null,
            string? role = null,
            int pageSize = 20,
            int pageNumber = 1,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks asynchronously whether a user with the specified <paramref name="userId"/> exists in the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to check.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains 
        /// <c>true</c> if a user with the given ID exists; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> CheckUserIdExistsAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a user along with their associated roles from the database.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user as a string. 
        /// This will be compared against the <see cref="User.UserId"/> property.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the <see cref="User"/> 
        /// object with its related roles if found; otherwise, <c>null</c>.
        /// </returns>
        public Task<UserDto?> GetUserWithRolesAsync(string userId, CancellationToken cancellationToken = default);
    }
}
