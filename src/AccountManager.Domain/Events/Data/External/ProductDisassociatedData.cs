// <copyright file="ProductDisassociatedData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.External
{
    /// <summary>
    /// Represents the payload for a product disassociated event.
    /// </summary>
    public class ProductDisassociatedData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated account.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the reason for disassociation.
        /// </summary>
        required public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the disassociation occurred.
        /// </summary>
        required public DateTime DisassociatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that performed the disassociation.
        /// </summary>
        required public string DisassociatedBy { get; set; }
    }
}
