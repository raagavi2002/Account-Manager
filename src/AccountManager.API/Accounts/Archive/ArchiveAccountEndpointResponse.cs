// <copyright file="ArchiveAccountEndpointResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Archive
{
    /// <summary>
    /// Represents the endpoint response if the account has been archived.
    /// </summary>
    public class ArchiveAccountEndpointResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the account has been archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was archived.
        /// </summary>
        public DateTime ArchivedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier (e.g., username or system actor) of the entity that archived the account.
        /// </summary>
        public string? ArchivedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the archival process is compliant with GDPR regulations.
        /// </summary>
        public bool IsGDPRComplaint { get; set; }

        /// <summary>
        /// Gets or sets the reason why the account has been archived.
        /// </summary>
        public string? Reason { get; set; }
    }
}
