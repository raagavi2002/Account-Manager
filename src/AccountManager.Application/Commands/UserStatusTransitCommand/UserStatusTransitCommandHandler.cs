// <copyright file="UserStatusTransitCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.UserStatusTransitCommand
{
    using System.Text.Json;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="UserStatusTransitCommand"/> request to transition a user's status
    /// between <c>Active</c> and <c>InActive</c>.
    /// </summary>
    /// <remarks>
    /// This handler validates the requested status transition, ensures compliance with business rules,
    /// and produces domain events for activation or deactivation. The resulting event is persisted
    /// to the outbox for Kafka publishing.
    /// </remarks>
    public class UserStatusTransitCommandHandler : IRequestHandler<UserStatusTransitCommand, UserStatusTransitCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventFactory domainEventFactory;
        private readonly IMediator mediator;

        public UserStatusTransitCommandHandler(IUnitOfWork unitOfWork, IDomainEventFactory domainEventFactory, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.domainEventFactory = domainEventFactory;
            this.mediator = mediator;
        }

        /// <summary>
        /// Processes the <see cref="UserStatusTransitCommand"/> request.
        /// </summary>
        /// <param name="request">
        /// The command containing the target user ID, desired status, and reason for the transition.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A <see cref="UserStatusTransitCommandResponse"/> containing the updated status,
        /// reason, and metadata about the transition.
        /// </returns>
        /// <exception cref="UserNotFoundException">
        /// Thrown when the specified user cannot be found in the repository.
        /// </exception>
        /// <exception cref="UserValidationException">
        /// Thrown when the target status is invalid, unchanged, or when a reason is not provided.
        /// </exception>
        public async Task<UserStatusTransitCommandResponse> Handle(UserStatusTransitCommand request, CancellationToken cancellationToken)
        {
            var userRepository = await unitOfWork.User.GetUserByIdAsync(request.UserId, cancellationToken);

            if (userRepository == null)
            {
                throw new UserNotFoundException(new Domain.Errors.ErrorResponses
                {
                    Code = "USER_NOT_FOUND",
                    Message = $"User with ID {request.UserId} was not found.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            var previousStatus = userRepository.IsActive
                ? EnumParser.GetEnumMemberValue(UserStatus.Active)
                : EnumParser.GetEnumMemberValue(UserStatus.InActive);

            if (string.Equals(request.TargetStatus.ToUpper(), previousStatus, StringComparison.OrdinalIgnoreCase))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "USER_STATUS_UNCHANGED",
                    Message = $"User with ID {request.UserId} is already in the target status '{request.TargetStatus}'.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            if (string.IsNullOrEmpty(request.TargetStatus) ||
                (!string.Equals(request.TargetStatus.ToUpper(), EnumParser.GetEnumMemberValue(UserStatus.Active).ToUpper(), StringComparison.OrdinalIgnoreCase) &&
                 !string.Equals(request.TargetStatus.ToUpper(), EnumParser.GetEnumMemberValue(UserStatus.InActive).ToUpper(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "INVALID_TARGET_STATUS",
                    Message = $"The target status '{request.TargetStatus}' is invalid. Valid values are 'Active' or 'InActive'.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            if (string.IsNullOrEmpty(request.Reason))
            {
                throw new UserValidationException(new Domain.Errors.ErrorResponses
                {
                    Code = "REASON_REQUIRED",
                    Message = "A reason for the status change is required.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            bool updatedStatus = string.Equals(request.TargetStatus.ToUpper(), EnumParser.GetEnumMemberValue(UserStatus.Active).ToUpper(), StringComparison.OrdinalIgnoreCase);
            await unitOfWork.User.UpdateUserStatusAsync(request.UserId, request.TargetStatus, cancellationToken);

            if (updatedStatus)
            {
                var eventData = domainEventFactory.CreateUserActivatedEvent(userRepository, request.Reason);
                KafkaProducedEventDto eventDto = new KafkaProducedEventDto
                {
                    EventType = EventTypes.UserActivated,
                    AccountId = userRepository.AccountId,
                    Payload = System.Text.Json.JsonSerializer.Serialize(eventData),
                    ProducerService = "AccountManagerService",
                    ProducedAt = DateTime.UtcNow,
                    CorrelationId = Guid.NewGuid(),
                    Status = OutboxStatus.Pending.ToString(),
                };
                await unitOfWork.Outbox.AddKafkaProducedEventAsync(eventDto, cancellationToken);
            }
            else
            {
                var eventData = domainEventFactory.CreateUserDeactivatedEvent(userRepository, request.Reason);
                KafkaProducedEventDto eventDto = new KafkaProducedEventDto
                {
                    EventType = EventTypes.UserDeactivated,
                    AccountId = userRepository.AccountId,
                    Payload = System.Text.Json.JsonSerializer.Serialize(eventData),
                    ProducerService = "AccountManagerService",
                    ProducedAt = DateTime.UtcNow,
                    CorrelationId = Guid.NewGuid(),
                    Status = OutboxStatus.Pending.ToString(),
                };
                await unitOfWork.Outbox.AddKafkaProducedEventAsync(eventDto, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Publish audit log
            await mediator.Send(
                new Commands.CreateAuditEntryCommand.CreateAuditEntryCommand
            {
                EntityType = Domain.Enums.AuditLog.AuditEntityType.User,
                EntityId = request.UserId,
                OperationType = updatedStatus ? Domain.Enums.AuditLog.AuditOperation.UserActivated : Domain.Enums.AuditLog.AuditOperation.UserDeactivated,
                UserId = Guid.NewGuid(),
                BeforeState = JsonSerializer.Serialize(userRepository),
                AfterState = JsonSerializer.Serialize(new { IsActive = updatedStatus }),
                Outcome = Domain.Enums.AuditLog.AuditOutcome.Success,
                Reason = request.Reason,
            }, cancellationToken);

            return new UserStatusTransitCommandResponse
            {
                UserId = request.UserId,
                Reason = request.Reason,
                IsActive = updatedStatus,
                StatusChangedAt = DateTime.UtcNow,
                StatusChangedBy = Guid.NewGuid(),
                Version = userRepository.Version + 1 ?? 0,
            };
        }
    }
}
