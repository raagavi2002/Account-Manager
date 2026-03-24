// <copyright file="FieldChangeDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    public class FieldChangeDto
    {
        /// <summary>
        /// Gets or sets the name of the field that was changed.
        /// </summary>
        required public string Field { get; set; }

        /// <summary>
        /// Gets or sets the previous value of the field.
        /// </summary>
        required public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value of the field.
        /// </summary>
        required public string NewValue { get; set; }
    }
}
