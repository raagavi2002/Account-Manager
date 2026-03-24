// <copyright file="ProductCreatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.IncomingEvents
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data.External;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an incoming event indicating a product was created externally.
    /// </summary>
    public class ProductCreatedEvent : BaseEvent<ProductCreatedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCreatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public ProductCreatedEvent()
        {
            this.EventType = EventTypes.ProductCreated;
        }
    }
}
