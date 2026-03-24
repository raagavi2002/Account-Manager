// <copyright file="ProductDisassociatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.IncomingEvents
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data.External;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an incoming event indicating a product was disassociated externally.
    /// </summary>
    public class ProductDisassociatedEvent
        : BaseEvent<ProductDisassociatedData>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ProductDisassociatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public ProductDisassociatedEvent()
        {
            this.EventType = EventTypes.ProductDisassociated;
        }
    }
}
