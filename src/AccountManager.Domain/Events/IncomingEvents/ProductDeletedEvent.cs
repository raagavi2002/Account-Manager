// <copyright file="ProductDeletedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.IncomingEvents
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data.External;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an incoming event indicating a product was deleted externally.
    /// </summary>
    public class ProductDeletedEvent : BaseEvent<ProductDeletedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDeletedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public ProductDeletedEvent()
        {
            this.EventType = EventTypes.ProductDeleted;
        }
    }
}
