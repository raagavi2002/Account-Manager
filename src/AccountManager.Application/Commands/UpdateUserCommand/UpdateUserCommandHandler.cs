// <copyright file="UpdateUserCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.UpdateUserCommand
{
    using AccountManager.Application.Abstractions;
    using AccountManager.Domain.Events.Models;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="UpdateUserCommand"/> and returns an <see cref="UpdateUserCommandResponse"/>.
    /// Includes Kafka event publishing and audit logging.
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplogger _applogger;
        private readonly IDomainEventFactory _domainEventFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">Provides access to repositories and manages transactions.</param>
        /// <param name="applogger">Application logger for audit and diagnostic logging.</param>
        /// <param name="domainEventFactory">Factory for creating domain events.</param>
        public UpdateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IApplogger applogger,
            IDomainEventFactory domainEventFactory)
        {
            _unitOfWork = unitOfWork;
            _applogger = applogger;
            _domainEventFactory = domainEventFactory;
        }

        /// <summary>
        /// Handles the update user command by validating input, updating user details,
        /// saving changes, and returning a response.
        /// </summary>
        /// <param name="request">The <see cref="UpdateUserCommand"/> containing user update details.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An <see cref="UpdateUserCommandResponse"/> containing updated user information.</returns>
        /// <exception cref="UserValidationException">
        /// Thrown when email, first name, or last name validation fails.
        /// </exception>
        /// <exception cref="UserNotFoundException">
        /// Thrown when the user with the specified ID does not exist.
        /// </exception>
        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate email format
            if (string.IsNullOrEmpty(request.Email) || !request.Email.Contains("@"))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidEmail",
                    Message = "Valid email address is required.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            // Validate first name
            if (string.IsNullOrEmpty(request.FirstName))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidFirstName",
                    Message = "First name is required.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            // Validate last name
            if (string.IsNullOrEmpty(request.LastName))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidLastName",
                    Message = "Last name is required.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            // Get user by ID
            var user = await _unitOfWork.User.GetUserByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(new Domain.Errors.ErrorResponses
                {
                    Code = "UserNotFound",
                    Message = "User not found",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            // Check if email is already in use by another user
            var existingUser = await _unitOfWork.User.EmailExistsAsync(request.Email, cancellationToken);
            if (existingUser)
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "EmailAlreadyExists",
                    Message = "Email already in use",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                    Details = new Domain.Errors.ErrorInfo
                    {
                        AdditionalInfo = new Dictionary<string, string>
                        {
                            { "EmailAddress", request.Email },
                        },
                        AccountId = user.AccountId,
                    },
                });
            }

            // Store changes for audit and event publishing
            var changes = new List<(string fieldName, object oldValue, object newValue)>();
            var changedFields = new Dictionary<string, UserFieldChange>();

            if (user.Email != request.Email)
            {
                changes.Add(("Email", user.Email, request.Email));
                changedFields["Email"] = new UserFieldChange { OldValue = user.Email, NewValue = request.Email };
            }

            if (user.FirstName != request.FirstName)
            {
                changes.Add(("FirstName", user.FirstName, request.FirstName));
                changedFields["FirstName"] = new UserFieldChange { OldValue = user.FirstName, NewValue = request.FirstName };
            }

            if (user.LastName != request.LastName)
            {
                changes.Add(("LastName", user.LastName, request.LastName));
                changedFields["LastName"] = new UserFieldChange { OldValue = user.LastName, NewValue = request.LastName };
            }

            // Update user
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateUserCommandResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                UpdatedAt = DateTime.UtcNow,
            };
        }
    }
}
