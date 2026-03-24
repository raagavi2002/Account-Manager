// <copyright file="UserFieldChange.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Models
{
    /// <summary>
    /// Represents a change to a single user field.
    /// </summary>
    public class UserFieldChange
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
