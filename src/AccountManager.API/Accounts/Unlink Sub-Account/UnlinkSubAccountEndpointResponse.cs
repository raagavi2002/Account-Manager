// <copyright file="UnlinkSubAccountEndpointResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Unlink_Sub_Account
{
    /// <summary>
    /// Represents the response returned after successfully unlinking a sub-account from a head account.
    /// </summary>
    public class UnlinkSubAccountEndpointResponse
    {
        /// <summary>
        /// Gets the unique identifier of the sub-account that was unlinked.
        /// </summary>
        /// <remarks>
        /// UUID representing the unlinked sub-account.
        /// </remarks>
        public Guid SubAccountId { get; init; }

        /// <summary>
        /// Gets the unique identifier of the former parent (head) account.
        /// </summary>
        /// <remarks>
        /// UUID representing the previous parent account.
        /// </remarks>
        public Guid FormerHeadAccountId { get; init; }

        /// <summary>
        /// Gets the UTC timestamp indicating when the unlinking occurred.
        /// </summary>
        public DateTime UnlinkedAt { get; init; }

        /// <summary>
        /// Gets the unique identifier of the administrator who performed the unlinking.
        /// </summary>
        /// <remarks>
        /// UUID of the admin user responsible for the action.
        /// </remarks>
        public Guid UnlinkedBy { get; init; }

        /// <summary>
        /// Gets the business reason provided for unlinking the sub-account.
        /// </summary>
        public string Reason { get; init; } = string.Empty;
    }
}
