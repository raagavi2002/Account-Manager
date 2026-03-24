// <copyright file="ProductUpdatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.IncomingEvents
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data.External;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an incoming event indicating a product was updated externally.
    /// </summary>
    public class ProductUpdatedEvent : BaseEvent<ProductUpdatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUpdatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public ProductUpdatedEvent()
        {
            this.EventType = EventTypes.ProductUpdated;
        }
    }
}
