// <copyright file="AccountUnlinkedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.PublishedEvents
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when accounts are unlinked.
    /// </summary>
    public class AccountUnlinkedEvent : BaseEvent<AccountUnlinkedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountUnlinkedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public AccountUnlinkedEvent()
        {
            EventType = EventTypes.AccountUnlinked;
        }
    }
}
