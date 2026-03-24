// <copyright file="UserStatusTransitEndpointRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.User_Status_Transit
{
    using AccountManager.API.Authorization;
    using AccountManager.Domain.Enums.Authorization;

    /// <summary>
    /// Represents the request payload for transitioning a user's status 
    /// between Active and InActive.
    /// </summary>
    public class UserStatusTransitEndpointRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to update.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the target status for the user.
        /// Expected values are "Active" or "InActive".
        /// </summary>
        public string? TargetStatus { get; set; }

        /// <summary>
        /// Gets or sets the business reason for the status change.
        /// This provides context for auditing and compliance purposes.
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Gets or sets the current version number for optimistic locking.
        /// Used to prevent conflicting updates when multiple requests occur simultaneously.
        /// </summary>
        public int? Version { get; set; }

        /// <summary>
        /// Gets the required permission string needed to perform this request.
        /// </summary>
        public string RequiredPermission => Permissions.Administrative.Update.AccountStatus;

        /// <summary>
        /// Gets the account identifier associated with the request.
        /// Returns null since this request does not operate on a specific account.
        /// </summary>
        string? IRequirePermission.AccountId => null;
    }
}
