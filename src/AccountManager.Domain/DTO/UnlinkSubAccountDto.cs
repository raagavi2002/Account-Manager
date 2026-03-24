// <copyright file="UnlinkSubAccountDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Data transfer object used to request the unlinking of a sub-account
    /// from a head account.
    /// </summary>
    public class UnlinkSubAccountDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the head account
        /// from which the sub-account will be unlinked.
        /// </summary>
        required public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the sub-account
        /// that is to be unlinked.
        /// </summary>
        required public Guid SubAccountId { get; set; }

        /// <summary>
        /// Gets or sets the reason for unlinking the sub-account.
        /// </summary>
        required public string Reason { get; set; }
    }
}
