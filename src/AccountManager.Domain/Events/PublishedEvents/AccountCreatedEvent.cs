// <copyright file="AccountCreatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when a new account is created.
    /// </summary>
    public class AccountCreatedEvent : BaseEvent<AccountCreatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountCreatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public AccountCreatedEvent()
        {
            // Initialize required properties with default values
            EventType = EventTypes.AccountCreated;
        }
    }
}
