// <copyright file="AccountLinkedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when accounts are linked.
    /// </summary>
    public class AccountLinkedEvent : BaseEvent<AccountLinkedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountLinkedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public AccountLinkedEvent()
        {
            this.EventType = EventTypes.AccountLinked;
        }
    }
}
