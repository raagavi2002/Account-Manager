// <copyright file="AccountStatusTransitDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Represents the data required to perform an account status transition.
    /// </summary>
    public class AccountStatusTransitDto
    {
        /// <summary>
        /// Gets or sets the identifier of the account.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the target status of the account.
        /// </summary>
        required public string AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets the reason for the account status transition.
        /// </summary>
        required public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the expected version of the account
        /// used for optimistic concurrency control.
        /// </summary>
        required public int Version { get; set; }
    }
}
