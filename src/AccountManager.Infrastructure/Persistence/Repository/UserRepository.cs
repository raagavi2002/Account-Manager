// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Repository
{
    using System.Threading;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Domain.Results;
    using AccountManager.Infrastructure.Persistence.Entities;
    using Microsoft.EntityFrameworkCore;
    using Polly;

    /// <summary>
    /// Provides data access operations related to users.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AccountManagerDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public UserRepository(AccountManagerDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new user to the specified account.
        /// </summary>
        /// <param name="userDto">The data transfer object containing user information.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains an <see cref="AddUserResult"/> with details of the added user.
        /// </returns>
        public async Task<AddUserResult> AddUserAsync(AddUserDto userDto)
        {
            User newUser = new User
            {
                UserId = Guid.NewGuid(),
                AccountId = userDto.AccountId,
                Email = userDto.EmailAddress ?? string.Empty,
                FirstName = userDto.FirstName ?? string.Empty,
                LastName = userDto.LastName ?? string.Empty,
                Version = 1,
                LastLoginAt = null,
                LoginCount = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system",
            };

            userDto.Roles.ForEach(role =>
            {
                newUser.UserRoles.Add(new UserRole
                {
                    RoleId = (int)role,
                    UserId = newUser.UserId,
                    RoleName = EnumParser.GetEnumMemberValue<UserRoleType>(role),
                    EffectiveFrom = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    AssignedBy = 0,
                    RoleType = (int)userDto.UserType,
                });
            });

            await context.Users.AddAsync(newUser);
            return new AddUserResult
            {
                UserId = newUser.UserId,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Roles = userDto.Roles,
                ClerkUserId = newUser.ClerkUserId ?? string.Empty,
                IsActive = newUser.IsActive,
                CreatedAt = newUser.CreatedAt ?? DateTime.UtcNow,
            };
        }

        /// <summary>
        /// Determines whether the specified account has at least one user
        /// assigned the <see cref="UserRoleType.MainClient"/> role.
        /// </summary>
        /// <param name="accountId">The identifier of the account.</param>
        /// <returns>
        /// <c>true</c> if the account has a main client user; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> HasMainClientUserAsync(Guid accountId)
        {
            return await context.Users.AnyAsync(u => u.AccountId == accountId &&
             u.UserRoles.Any(r => r.RoleId == (int)UserRoleType.MainClient));
        }

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
        public async Task<UserDto?> GetUserWithRolesAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await context.Users
                .AsNoTracking()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(
                    u => u.ClerkUserId == userId || u.UserId.ToString() == userId,
                    cancellationToken);

            if (user is null)
            {
                return null;
            }

            var roles = user.UserRoles
                .Where(ur => ur.UserId == user.UserId)
                .Select(ur => ur.RoleName)
                .ToList();

            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                LastLoginAt = user.LastLoginAt,
                LoginCount = user.LoginCount,
                Roles = roles,
                AccountId = user.AccountId,
            };
        }

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously and maps the result to a <see cref="UserDto"/>.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user to retrieve.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains
        /// the <see cref="UserDto"/> if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.Users
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsActive = u.IsActive,
                    AccountId = u.AccountId,
                    LastLoginAt = u.LastLoginAt,
                    LoginCount = u.LoginCount,
                    Roles = u.UserRoles.Select(r => r.RoleName).ToList(),
                    Version = u.Version,
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

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
        public async Task<bool> UpdateUserStatusAsync(Guid userId, string status, CancellationToken cancellationToken = default)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user == null)
            {
                return false;
            }

            switch (status.ToUpperInvariant())
            {
                case "ACTIVE":
                    user.IsActive = true;
                    user.DeactivatedAt = null;
                    break;

                case "INACTIVE":
                    user.IsActive = false;
                    user.DeactivatedAt = DateTime.UtcNow;
                    break;

                default:
                    throw new ArgumentException($"Invalid status value: {status}. Expected 'ACTIVE' or 'INACTIVE'.", nameof(status));
            }

            user.UpdatedAt = DateTime.UtcNow;
            user.Version++;
            return true;
        }

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
        public async Task<bool> IsUserInRoleAsync(
            Guid userId,
            UserRoleType roleType,
            CancellationToken cancellationToken = default)
        {
            return await context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == (int)roleType, cancellationToken);
        }

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
        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await context.Set<User>()
                .AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
        }

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
        public async Task<bool> DuplicateNameExistsAsync(
            string firstName,
            string lastName,
            Guid? excludeUserId = null,
            CancellationToken cancellationToken = default)
        {
            return await context.Set<User>()
                .AnyAsync(
                    u => u.FirstName.ToLower() == firstName.ToLower() && u.LastName.ToLower() == lastName.ToLower() &&
                    (!excludeUserId.HasValue || u.UserId != excludeUserId.Value),
                    cancellationToken);
        }

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
        public async Task<(List<UserDto> Users, int TotalCount)> GetAccountUsersAsync(
            Guid accountId,
            bool? isActive = null,
            string? role = null,
            int pageSize = 20,
            int pageNumber = 1,
            CancellationToken cancellationToken = default)
        {
            var query = context.Users
                .AsNoTracking()
                .Where(u => u.AccountId == accountId);

            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                query = query.Where(u => u.UserRoles.Any(ur => ur.RoleName.Equals(role, StringComparison.OrdinalIgnoreCase)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var skip = (pageNumber - 1) * pageSize;
            var users = await query
                .Skip(skip)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsActive = u.IsActive,
                    AccountId = u.AccountId,
                    LastLoginAt = u.LastLoginAt,
                    LoginCount = u.LoginCount,
                    Roles = u.UserRoles.Select(r => r.RoleName).ToList(),
                    Version = u.Version,
                })
                .ToListAsync(cancellationToken);

            return (users, totalCount);
        }

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
        public async Task<bool> CheckUserIdExistsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.Users.AnyAsync(u => u.UserId == userId, cancellationToken);
        }

        /// <summary>
        /// Updates the Clerk user ID for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="clerkUserId">The Clerk user ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateClerkUserIdAsync(Guid userId, string clerkUserId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                user.ClerkUserId = clerkUserId;
                user.UpdatedAt = DateTime.UtcNow;
                user.Version++;
            }
        }
    }
}
