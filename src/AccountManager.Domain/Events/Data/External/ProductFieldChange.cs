// <copyright file="ProductFieldChange.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Data.External
{
    /// <summary>
    /// Represents a change to a single product field.
    /// </summary>
    public class ProductFieldChange
    {
        /// <summary>
        /// Gets or sets the previous value of the field.
        /// </summary>
        required public object OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value of the field.
        /// </summary>
        required public object NewValue { get; set; }
    }
}
