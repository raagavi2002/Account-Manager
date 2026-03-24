// <copyright file="UserDeactivatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when a user is deactivated.
    /// </summary>
    public class UserDeactivatedEvent : BaseEvent<UserDeactivatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDeactivatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public UserDeactivatedEvent()
        {
            this.EventType = EventTypes.UserDeactivated;
        }
    }
}
