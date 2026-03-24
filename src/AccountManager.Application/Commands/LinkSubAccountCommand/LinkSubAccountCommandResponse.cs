// <copyright file="LinkSubAccountCommandResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.LinkSubAccountCommand
{
    /// <summary>
    /// Represents the response returned after successfully creating
    /// a head–sub account relationship.
    /// </summary>
    /// <remarks>
    /// This response contains identifiers for the relationship, the
    /// head account, and the sub-account, along with audit metadata
    /// describing when and by whom the relationship was created.
    /// </remarks>
    public class LinkSubAccountCommandResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the created relationship.
        /// </summary>
        public int RelationshipId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the head (parent) account.
        /// </summary>
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the sub (child) account.
        /// </summary>
        public Guid SubAccountId { get; set; }

        /// <summary>
        /// Gets or sets the type of relationship that was created.
        /// </summary>
        /// <remarks>
        /// For head–sub account links, this value will be <c>HEAD_SUB</c>.
        /// </remarks>
        public string? RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the relationship was created.
        /// </summary>
        public DateTime LinkedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier (such as email or username)
        /// of the user who created the relationship.
        /// </summary>
        public string? LinkedBy { get; set; }
    }
}
