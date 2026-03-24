// <copyright file="LinkSubAccountResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Link_Sub_Account
{
    /// <summary>
    /// Represents the response returned after successfully creating
    /// a head-sub account relationship.
    /// </summary>
    public class LinkSubAccountResponse
    {
        /// <summary>
        /// Gets or sets Unique identifier of the created account relationship.
        /// </summary>
        public int RelationshipId { get; set; }

        /// <summary>
        /// Gets or sets Unique identifier of the sub-account that was linked.
        /// </summary>
        public Guid SubAccountId { get; set; }

        /// <summary>
        /// Gets or sets Unique identifier of the head account to which the sub-account is linked.
        /// </summary>
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets Type of relationship created between the head and sub account.
        /// </summary>
        /// <remarks>
        /// Will always be <c>HEAD_SUB</c> for this endpoint.
        /// </remarks>
        public string RelationshipType { get; set; } = "HEAD_SUB";

        /// <summary>
        /// Gets or sets UTC timestamp indicating when the relationship was created.
        /// </summary>
        public DateTime LinkedAt { get; set; }

        /// <summary>
        /// Gets or sets Identifier (usually email or username) of the user who created
        /// the relationship.
        /// </summary>
        public string LinkedBy { get; set; } = default!;
    }
}
