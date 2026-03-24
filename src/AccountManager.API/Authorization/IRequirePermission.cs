// <copyright file="IRequirePermission.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Authorization
{
    /// <summary>
    /// Marker interface for requests that require permission validation
    /// </summary>
    public interface IRequirePermission
    {
        /// <summary>
        /// Gets the permission required to execute this request.
        /// </summary>
        string RequiredPermission { get; }

        /// <summary>
        /// Gets The account ID context for the request
        /// Optional for Admin users, required for others.
        /// </summary>
        string? AccountId { get; }
    }
}
