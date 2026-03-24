// <copyright file="DomainEventFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Factories
{
    using System;
    using AccountManager.Application.Events;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;
    using AccountManager.Domain.Events.Published;
    using AccountManager.Domain.Events.PublishedEvents;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Domain.Results;

    /// <summary>
    /// Factory for creating and publishing domain events.
    /// </summary>
    internal class DomainEventFactory : IDomainEventFactory
    {
        private readonly IEventFactory eventFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventFactory"/> class.
        /// </summary>
        /// <param name="eventFactory">The event factory to use for event creation.</param>
        public DomainEventFactory(IEventFactory eventFactory)
        {
            this.eventFactory = eventFactory;
        }

        /// <summary>
        /// Creates an <see cref="AccountCreatedEvent"/> for the specified account.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="accountDto">The data transfer object containing account details.</param>
        /// <returns>An instance of <see cref="AccountCreatedEvent"/> representing the account creation event.</returns>
        public AccountCreatedEvent CreateAccountCreatedEvent(Guid accountId, CreateAccountDto accountDto)
        {
            var accountEventData = new AccountCreatedData
            {
                Account = new AccountData
                {
                    AccountId = accountId,
                    AccountName = accountDto?.AccountName ?? string.Empty,
                    AccountType = accountDto?.AccountType ?? string.Empty,
                    Currency = accountDto?.Currency ?? string.Empty,
                    TimezoneId = accountDto?.Timezone.ToString() ?? string.Empty,
                    Status = "Active",
                    Address = new AddressDto
                    {
                        Street = accountDto?.Address?.Street ?? string.Empty,
                        Street2 = accountDto?.Address?.Street2 ?? string.Empty,
                        City = accountDto?.Address?.City ?? string.Empty,
                        State = accountDto?.Address?.State ?? string.Empty,
                        PostalCode = accountDto?.Address?.PostalCode ?? string.Empty,
                        Country = accountDto?.Address?.Country ?? string.Empty,
                    },
                    VatNumber = accountDto?.VatNumber ?? string.Empty,
                    AccountManagerId = accountDto?.AccountManagerId,
                    CsmId = accountDto?.CsmId,
                    IsHeadAccount = !string.IsNullOrEmpty(accountDto?.HeadAccountId.ToString()),
                    HeadAccountId = accountDto?.HeadAccountId,
                    InvoiceEmail = accountDto?.InvoiceEmailAddress,
                    InvoiceType = accountDto?.InvoiceType,
                    NotificationEmailAddress = accountDto?.NotificationEmailAddress,
                },
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system",
            };
            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateAccountCreatedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };
            var eventInstance = eventFactory.CreateEvent<AccountCreatedEvent, AccountCreatedData>(accountEventData, eventData);

            return eventInstance;
        }

        /// <summary>
        /// Creates an <see cref="AccountStatusChangedEvent"/> representing a change in an account's status.
        /// </summary>
        /// <param name="accountStatusTransitDto">
        /// The DTO containing the new status transition information, including the account ID, reason, and version.
        /// </param>
        /// <param name="accountDto">
        /// Optional additional account information. Can be <c>null</c> if not available.
        /// Used to populate details like the account name in the event.
        /// </param>
        /// <returns>
        /// An <see cref="AccountStatusChangedEvent"/> object containing the account change data and metadata
        /// for domain event handling.
        /// </returns>
        public AccountStatusChangedEvent CreateAccountStatusChangedEvent(
            AccountStatusTransitDto accountStatusTransitDto,
            AccountDto? accountDto)
        {
            var accountChangedData = new AccountStatusChangedData
            {
                AccountId = accountStatusTransitDto.AccountId,
                AccountName = accountDto?.AccountName ?? string.Empty,
                PreviousStatus = accountDto?.AccountStatus?.ToUpper() ?? string.Empty,
                NewStatus = accountStatusTransitDto.AccountStatus.ToUpper(),
                Reason = accountStatusTransitDto.Reason,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = "system",
                Version = accountStatusTransitDto.Version,
            };

            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateAccountStatusChangedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };

            var eventInstance = eventFactory.CreateEvent<AccountStatusChangedEvent, AccountStatusChangedData>(accountChangedData, eventData);
            return eventInstance;
        }

        /// <summary>
        /// Creates an <see cref="AccountLinkedEvent"/> representing the linking of a sub-account to a head account.
        /// </summary>
        /// <param name="accountResult">
        /// The result of the sub-account linking operation, containing the head account ID, sub-account ID, timestamp, and linked-by information.
        /// </param>
        /// <param name="subAccountName">The name of the sub-account being linked.</param>
        /// <returns>
        /// An <see cref="AccountLinkedEvent"/> object containing the account linked data and metadata for domain event handling.
        /// </returns>
        public AccountLinkedEvent CreateAccountLinkedEvent(LinkSubAccountResult accountResult, string subAccountName)
        {
            AccountLinkedData accountLinkedData = new AccountLinkedData
            {
                HeadAccountId = accountResult.HeadAccountId,
                SubAccountId = accountResult.SubAccountId,
                SubAccountName = subAccountName,
                LinkedAt = accountResult.LinkedAt,
                LinkedBy = accountResult.LinkedBy ?? string.Empty,
            };

            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateAccountLinkedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };

            var eventInstance = eventFactory.CreateEvent<AccountLinkedEvent, AccountLinkedData>(accountLinkedData, eventData);
            return eventInstance;
        }

        /// <summary>
        /// Creates an <see cref="AccountUpdatedEvent"/> representing an update to an account.
        /// </summary>
        /// <param name="updateAccountDto">The DTO containing updated account information.</param>
        /// <param name="fieldChanges">A list of field changes describing what was updated.</param>
        /// <returns>
        /// An <see cref="AccountUpdatedEvent"/> object containing the account update data and metadata
        /// for domain event handling.
        /// </returns>
        public AccountUpdatedEvent CreateAccountUpdatedEvent(UpdateAccountDto updateAccountDto, List<FieldChangeDto> fieldChanges)
        {
            AccountUpdatedData accountUpdatedData = new AccountUpdatedData
            {
                AccountId = updateAccountDto.AccountId ?? Guid.Empty,
                AccountName = updateAccountDto.AccountName ?? string.Empty,
                ChangedFields = fieldChanges.Select(fc =>
                    new FieldChange
                    {
                        Field = fc.Field,
                        OldValue = fc.OldValue,
                        NewValue = fc.NewValue,
                    }
                ).ToList(),
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "system",
                ChangeType = "FieldUpdate",
                ChangeReason = "Updated via API",
                Version = updateAccountDto.Version,
            };
            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateAccountUpdatedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };
            var eventInstance = eventFactory.CreateEvent<AccountUpdatedEvent, AccountUpdatedData>(accountUpdatedData, eventData);
            return eventInstance;
        }

        /// <summary>
        /// Creates an <see cref="AccountUnlinkedEvent"/> instance containing metadata and details
        /// about a sub-account being unlinked from its head account.
        /// </summary>
        /// <param name="accountResult">
        /// The result of the unlink operation, containing identifiers, timestamps, and contextual information
        /// about the head and sub-account involved.
        /// </param>
        /// <param name="subAccountName">
        /// The human-readable name of the sub-account that was unlinked.
        /// </param>
        /// <returns>
        /// A fully constructed <see cref="AccountUnlinkedEvent"/> populated with <see cref="AccountUnlinkedData"/>
        /// and associated <see cref="Models.EventMetadata"/>.
        /// </returns>
        public AccountUnlinkedEvent CreateAccountUnlinkedEvent(UnlinkSubAccountResult accountResult, string subAccountName)
        {
            AccountUnlinkedData accountUnlinkedData = new AccountUnlinkedData()
            {
                HeadAccountId = accountResult.FormerHeadAccountId,
                SubAccountId = accountResult.SubAccountId,
                SubAccountName = subAccountName,
                UnlinkedAt = accountResult.UnlinkedAt,
                UnlinkedBy = accountResult.UnlinkedBy.ToString(),
                Reason = accountResult.Reason,
            };
            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateAccountUnlinkedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };
            var eventInstance = eventFactory.CreateEvent<AccountUnlinkedEvent, AccountUnlinkedData>(accountUnlinkedData, eventData);
            return eventInstance;
        }

        /// <summary>
        /// Creates a <see cref="UserCreatedEvent"/> instance based on the provided user data.
        /// </summary>
        /// <param name="userDto">
        /// The data transfer object containing user information used to populate the event.
        /// </param>
        /// <param name="updatedStatus">
        /// A value indicating the updated active status of the user.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="UserCreatedEvent"/> containing the user creation details
        /// and associated metadata.
        /// </returns>
        /// <remarks>
        /// This method constructs the event payload (<see cref="UserCreatedData"/>) and metadata
        /// (<see cref="Models.EventMetadata"/>) before delegating to the event factory to create
        /// the final event instance.
        /// </remarks>
        public UserCreatedEvent CreateUserCreatedEvent(AddUserDto userDto, Guid userId)
        {
            var userCreatedData = new UserCreatedData
            {
                UserId = userId,
                Email = userDto.EmailAddress,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName ?? string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system",
                AccountId = userDto.AccountId,
                Roles = userDto.Roles,
                ClerkUserId = userId.ToString(),
            };

            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateUserCreatedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };

            var eventInstance = eventFactory.CreateEvent<UserCreatedEvent, UserCreatedData>(userCreatedData, eventData);
            return eventInstance;
        }

        /// <summary>
        /// Creates a <see cref="UserActivatedEvent"/> for the specified user.
        /// </summary>
        /// <param name="userDto">
        /// The user data transfer object containing user details such as UserId, Email, and AccountId.
        /// </param>
        /// <param name="reason">
        /// The reason for the activation, used for auditing and traceability.
        /// </param>
        /// <returns>
        /// A <see cref="UserActivatedEvent"/> populated with user activation data and event metadata.
        /// </returns>
        public UserActivatedEvent CreateUserActivatedEvent(UserDto userDto, string reason)
        {
            var userActivatedData = new UserActivatedData
            {
                UserId = userDto.UserId,
                Email = userDto.Email,
                ActivatedAt = DateTime.UtcNow,
                ActivatedBy = "system",
                AccountId = userDto.AccountId,
                Reason = reason,
            };

            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateUserActivatedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };

            return eventFactory.CreateEvent<UserActivatedEvent, UserActivatedData>(userActivatedData, eventData);
        }

        /// <summary>
        /// Creates a <see cref="UserDeactivatedEvent"/> for the specified user.
        /// </summary>
        /// <param name="userDto">
        /// The user data transfer object containing user details such as UserId, Email, and AccountId.
        /// </param>
        /// <param name="reason">
        /// The reason for the deactivation, used for auditing and traceability.
        /// </param>
        /// <returns>
        /// A <see cref="UserDeactivatedEvent"/> populated with user deactivation data and event metadata.
        /// </returns>
        public UserDeactivatedEvent CreateUserDeactivatedEvent(UserDto userDto, string reason)
        {
            var userDeactivatedData = new UserDeactivatedData
            {
                UserId = userDto.UserId,
                Email = userDto.Email,
                DeactivatedAt = DateTime.UtcNow,
                DeactivatedBy = "system",
                AccountId = userDto.AccountId,
                Reason = reason,
            };

            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateUserDeactivatedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };

            return eventFactory.CreateEvent<UserDeactivatedEvent, UserDeactivatedData>(userDeactivatedData, eventData);
        }

        /// <summary>
        /// Creates an <see cref="AuditEntryCreatedEvent"/> for the specified audit log entry.
        /// </summary>
        /// <param name="auditLog">The audit log entry.</param>
        /// <returns>An instance of <see cref="AuditEntryCreatedEvent"/> representing the audit entry creation event.</returns>
        public AuditEntryCreatedEvent CreateAuditEntryCreatedEvent(Aggregates.AuditAggregate.AuditLog auditLog)
        {
            var auditData = new Data.AuditEntryCreatedData
            {
                AuditId = auditLog.Id,
                EntityType = auditLog.EntityType,
                EntityId = auditLog.EntityId,
                RelatedEntityId = auditLog.RelatedEntityId,
                OperationType = auditLog.OperationType,
                UserId = auditLog.UserId,
                OccurredAtUtc = auditLog.OccurredAtUtc,
                Outcome = auditLog.Outcome,
                Reason = auditLog.Reason,
                BeforeState = auditLog.BeforeState,
                AfterState = auditLog.AfterState,
                ChangedFields = auditLog.ChangedFields,
                CorrelationId = auditLog.CorrelationId,
            };
            var eventData = auditLog.Metadata ?? new Models.EventMetadata();
            var eventInstance = eventFactory.CreateEvent<PublishedEvents.AuditEntryCreatedEvent, Data.AuditEntryCreatedData>(auditData, eventData);
            return eventInstance;
        }

        public UserCreatedEvent CreateUserCreatedEvent(AddUserDto userDto, bool updatedStatus)
        {
            UserCreatedData userCreatedData = new UserCreatedData
            {
                UserId = Guid.NewGuid(),
                Email = userDto.EmailAddress,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName ?? string.Empty,
                IsActive = updatedStatus,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system",
                AccountId = userDto.AccountId,
                Roles = userDto.Roles,
                ClerkUserId = Guid.NewGuid().ToString(),
            };

            var eventData = new Models.EventMetadata
            {
                Class = nameof(DomainEventFactory),
                Project = nameof(AccountManager.Domain),
                Method = nameof(this.CreateUserCreatedEvent),
                TraceId = Guid.NewGuid().ToString(),
            };

            var eventInstance = eventFactory.CreateEvent<UserCreatedEvent, UserCreatedData>(userCreatedData, eventData);
            return eventInstance;
        }
    }
}
