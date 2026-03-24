// <copyright file="StatusTransitRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Status_Transit
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents a request to change the status of an account.
    /// Implements <see cref="IRequirePermission"/> to enforce authorization requirements.
    /// </summary>
    public class StatusTransitRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account to be updated.
        /// </summary>
        /// <returns>A <see cref="Guid"/> representing the account ID.</returns>
        [FromRoute]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the target status of the account.
        /// </summary>
        /// <returns>A <see cref="string"/> representing the new account status.</returns>
        required public string AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets the reason for the account status transition.
        /// </summary>
        /// <returns>A <see cref="string"/> describing the reason for the change.</returns>
        required public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the expected version of the account.
        /// Used for optimistic concurrency control to ensure updates are applied correctly.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the account version.</returns>
        required public int Version { get; set; }

        /// <summary>
        /// Gets the permission required to perform the account status update.
        /// </summary>
        /// <returns>A <see cref="string"/> representing the required permission key.</returns>
        public string RequiredPermission => Permissions.Administrative.Update.AccountStatus;

        /// <summary>
        /// Gets the account identifier as a string for permission checks.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the account ID.</returns>
        string? IRequirePermission.AccountId => AccountId.ToString();
    }
}
