// <copyright file="ArchiveAccountDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Data transfer object used to request the archival of an account.
    /// </summary>
    public class ArchiveAccountDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account to be archived.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the archival request
        /// is made due to GDPR requirements.
        /// </summary>
        public bool IsGdprRequest { get; set; }

        /// <summary>
        /// Gets or sets the reason for archiving the account.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}
