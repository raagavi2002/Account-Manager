// <copyright file="UserActivatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when a user is activated.
    /// </summary>
    public class UserActivatedEvent : BaseEvent<UserActivatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserActivatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public UserActivatedEvent()
        {
            this.EventType = EventTypes.UserActivated;
        }
    }
}
