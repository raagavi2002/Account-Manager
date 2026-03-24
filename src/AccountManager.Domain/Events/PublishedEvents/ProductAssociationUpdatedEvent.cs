// <copyright file="ProductAssociationUpdatedEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Published
{
    using System.Diagnostics.CodeAnalysis;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Data;
    using AccountManager.Domain.Events.Models;

    /// <summary>
    /// Represents an event published when a product association is updated.
    /// </summary>
    public class ProductAssociationUpdatedEvent
        : BaseEvent<ProductAssociationUpdatedData>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ProductAssociationUpdatedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public ProductAssociationUpdatedEvent()
        {
            this.EventType = EventTypes.ProductAssociationUpdated;
        }
    }
}
