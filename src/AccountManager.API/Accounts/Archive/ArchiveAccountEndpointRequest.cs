// <copyright file="ArchiveAccountEndpointRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Archive
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents a request to archive an account, containing necessary information
    /// such as account identifier, GDPR compliance flag, and reason for archival.
    /// </summary>
    public class ArchiveAccountEndpointRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account to be archived.
        /// </summary>
        [FromRoute]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the archival request
        /// is made due to GDPR requirements.
        /// </summary>
        public bool IsGdprRequest { get; set; }

        /// <summary>
        /// Gets or sets the reason for archiving the account.
        /// This may include business, compliance, or user-requested reasons.
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Gets the permission required to perform the archival operation.
        /// </summary>
        public string RequiredPermission => Permissions.Administrative.Update.AccountStatus;

        /// <summary>
        /// Gets the account identifier as a string for permission validation.
        /// </summary>
        string? IRequirePermission.AccountId => AccountId.ToString();
    }
}
