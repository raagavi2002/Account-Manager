namespace AccountManager.API.Accounts.Status_Transit
{
    /// <summary>
    /// Represents the response returned after an account status transition command has been successfully processed.
    /// </summary>
    public class StatusTransitResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the current status of the account.
        /// </summary>
        required public string AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account status was changed.
        /// </summary>
        public DateTime StatusChangedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system
        /// that performed the status change.
        /// </summary>
        public string StatusChangedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the current version of the account entity.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the reason for the account status change.
        /// </summary>
        required public string Reason { get; set; }
    }
}
