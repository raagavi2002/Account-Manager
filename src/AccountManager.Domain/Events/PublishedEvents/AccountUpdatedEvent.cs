// <copyright file="AccountUpdatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when an account is updated.
    /// </summary>
    public class AccountUpdatedEvent : BaseEvent<AccountUpdatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountUpdatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public AccountUpdatedEvent()
        {
            this.EventType = EventTypes.AccountUpdated;
        }
    }
}
