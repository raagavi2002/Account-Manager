// <copyright file="AccountStatusChangedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when an account status changes.
    /// </summary>
    public class AccountStatusChangedEvent : BaseEvent<AccountStatusChangedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountStatusChangedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public AccountStatusChangedEvent()
        {
            this.EventType = EventTypes.AccountStatusChanged;
        }
    }
}
