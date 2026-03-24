// <copyright file="LinkSubAccountDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.DTO
{
    /// <summary>
    /// Represents the data required for link sub account to head account.
    /// </summary>
    public class LinkSubAccountDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the head (parent) account.
        /// </summary>
        /// <remarks>
        /// This value is supplied via the route parameter
        /// <c>{headAccountId}</c> in the request URL.
        /// </remarks>
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the sub (child) account.
        /// </summary>
        /// <remarks>
        /// The specified account will be linked as a sub-account of the
        /// head account provided in the route.
        /// </remarks>
        public Guid SubAccountId { get; set; }

        /// <summary>
        /// Gets or sets the type of relationship to be created.
        /// </summary>
        /// <remarks>
        /// Currently, only the value <c>HEAD_SUB</c> is supported.
        /// This property defaults to <c>HEAD_SUB</c> if not explicitly provided.
        /// </remarks>
        public string RelationshipType { get; set; } = "HEAD_SUB";
    }
}
