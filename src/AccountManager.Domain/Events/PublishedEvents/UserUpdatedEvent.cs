// <copyright file="UserUpdatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when a user is updated.
    /// </summary>
    public class UserUpdatedEvent : BaseEvent<UserUpdatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserUpdatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public UserUpdatedEvent()
        {
            this.EventType = EventTypes.UserUpdated;
        }
    }
}
