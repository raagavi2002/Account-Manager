// <copyright file="IDomainEventFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Interfaces
{
    using System.Reflection;
    using AccountManager.Application.Events;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Factories;
    using AccountManager.Domain.Events.Models;
    using AccountManager.Domain.Events.Published;
    using AccountManager.Domain.Events.PublishedEvents;
    using AccountManager.Domain.Results;

    /// <summary>
    /// Provides methods for creating and publishing domain events related to account management.
    /// </summary>
    public interface IDomainEventFactory
    {
        /// <summary>
        /// Creates an AccountCreatedEvent.
        /// </summary>
        /// <param name="accountId">The unique identifier of the created account.</param>
        /// <param name="accountDto">The data transfer object containing account details.</param>
        /// <returns>A fully populated AccountCreatedEvent.</returns>
        AccountCreatedEvent CreateAccountCreatedEvent(Guid accountId, CreateAccountDto accountDto);

        /// <summary>
        /// Creates an <see cref="AccountStatusChangedEvent"/> representing a change in the account's status.
        /// </summary>
        /// <param name="accountStatusTransitDto">The DTO containing the new status transition information.</param>
        /// <param name="accountDto">Optional additional account information. Can be <c>null</c> if not available.</param>
        /// <returns>
        /// An <see cref="AccountStatusChangedEvent"/> object representing the account status change,
        /// including all relevant information for domain event handling.
        /// </returns>
        AccountStatusChangedEvent CreateAccountStatusChangedEvent(AccountStatusTransitDto accountStatusTransitDto, AccountDto? accountDto);

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
        public AccountLinkedEvent CreateAccountLinkedEvent(LinkSubAccountResult accountResult, string subAccountName);

        /// <summary>
        /// Creates an <see cref="AccountUpdatedEvent"/> representing an update to an account.
        /// </summary>
        /// <param name="updateAccountDto">The DTO containing updated account information.</param>
        /// <param name="fieldChanges">A list of field changes describing what was updated.</param>
        /// <returns>
        /// An <see cref="AccountUpdatedEvent"/> object containing the account update data and metadata
        /// for domain event handling.
        /// </returns>
        public AccountUpdatedEvent CreateAccountUpdatedEvent(UpdateAccountDto updateAccountDto, List<FieldChangeDto> fieldChanges);

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
        public AccountUnlinkedEvent CreateAccountUnlinkedEvent(UnlinkSubAccountResult accountResult, string subAccountName);

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
        public UserCreatedEvent CreateUserCreatedEvent(AddUserDto userDto, bool updatedStatus);


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
        public UserActivatedEvent CreateUserActivatedEvent(UserDto userDto, string reason);

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
        public UserDeactivatedEvent CreateUserDeactivatedEvent(UserDto userDto, string reason);
    }
}
