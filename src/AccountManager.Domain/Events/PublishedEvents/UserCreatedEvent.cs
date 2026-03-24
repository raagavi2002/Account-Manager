// <copyright file="UserCreatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when a user is created.
    /// </summary>
    public class UserCreatedEvent : BaseEvent<UserCreatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public UserCreatedEvent()
        {
            this.EventType = EventTypes.UserCreated;
        }
    }
}
