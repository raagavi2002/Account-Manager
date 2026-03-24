/// <copyright file="AddUserCommandHandler.cs" company="PlaceholderCompany">
/// Copyright (c) PlaceholderCompany. All rights reserved.
/// </copyright>

namespace AccountManager.Application.Commands.AddUserCommand
{
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Authorization;
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;
    using System.Linq;

    /// <summary>
    /// Handles the AddUserCommand and returns an AddUserCommandResponse.
    /// </summary>
    public class AddUserCommandHandler(IDomainEventFactory domainEventFactory, IUnitOfWork unitOfWork, IApplogger applogger, IPermissionCalculator permissionCalculator, IClerkService clerkService) : IRequestHandler<AddUserCommand, AddUserCommandResponse>
    {
        /// <summary>
        /// Handles the AddUserCommand request.
        /// </summary>
        /// <param name="request">The AddUserCommand request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>An AddUserCommandResponse.</returns>
        public async Task<AddUserCommandResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AddUser.EmailAddress))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidEmail",
                    Message = "Email address is required.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            if (string.IsNullOrEmpty(request.AddUser.FirstName))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidFirstName",
                    Message = "First name is required.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            if (string.IsNullOrEmpty(request.AddUser.LastName))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidLastName",
                    Message = "Last name is required.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            if (await unitOfWork.User.EmailExistsAsync(request.AddUser.EmailAddress))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "EmailAlreadyExists",
                    Message = "A user with the same email address already exists.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                    Details = new Domain.Errors.ErrorInfo
                    {
                       AdditionalInfo = new Dictionary<string, string>
                       {
                           { "EmailAddress", request.AddUser.EmailAddress },
                       },
                       AccountId = request.AddUser.AccountId,
                    },
                });
            }

            if (await unitOfWork.User.DuplicateNameExistsAsync(request.AddUser.FirstName, request.AddUser.LastName))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "NameAlreadyExists",
                    Message = "A user with the same name already exists.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                    Details = new Domain.Errors.ErrorInfo
                    {
                        AdditionalInfo = new Dictionary<string, string>
                       {
                           { "First Name", request.AddUser.FirstName },
                           { "Last Name", request.AddUser.LastName },
                       },
                        AccountId = request.AddUser.AccountId,
                    },
                });
            }

            if (request.AddUser.Roles == null || !request.AddUser.Roles.Any())
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidRoles",
                    Message = "At least one role is required.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            var permissions = permissionCalculator.Validate(request.AddUser.Roles, Guid.Empty);

            if (!permissions.IsValid)
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidRolesCombination",
                    Message = "Error in role combination found.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                    Details = new Domain.Errors.ErrorInfo
                    {
                        AdditionalInfo = permissions.ValidationMessages,
                        AccountId = request.AddUser.AccountId,
                    },
                });
            }

            var userInfo = await unitOfWork.User.AddUserAsync(request.AddUser).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Create user in Clerk
            try
            {
                var adUser = new AdUser
                {
                    ObjectGuid = userInfo.UserId,
                    Username = request.AddUser.EmailAddress ?? $"{request.AddUser.FirstName}.{request.AddUser.LastName}".ToLower(),
                    FirstName = request.AddUser.FirstName ?? string.Empty,
                    LastName = request.AddUser.LastName ?? string.Empty,
                    Password = GenerateSecurePassword(),
                    Email = request.AddUser.EmailAddress,
                    Roles = request.AddUser.Roles.Select(r => r.ToString()).ToList(),
                };

                // TODO: Get orgId and role from configuration or request
                // string orgId = "org_38xynLE7Corb6vXciKyLh3n4jcy"; // From Postman
                // string role = "org:admin"; // From Postman

                var clerkUserId = await clerkService.CreateUserAsync(adUser, cancellationToken: cancellationToken);

                // Update DB with Clerk user ID
                await unitOfWork.User.UpdateClerkUserIdAsync(userInfo.UserId, clerkUserId);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                applogger.LogError("Failed to create user in Clerk", ex);
                // Continue without Clerk user, as DB user is already created
            }

            return new AddUserCommandResponse
            {
                UserId = userInfo.UserId,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Email = userInfo.Email,
                CreatedAt = userInfo.CreatedAt,
                IsActive = userInfo.IsActive,
                Roles = userInfo.Roles,
                ClerkUserId = userInfo.ClerkUserId,
            };
        }

        private static string GenerateSecurePassword()
        {
            // Generate a secure temporary password
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
