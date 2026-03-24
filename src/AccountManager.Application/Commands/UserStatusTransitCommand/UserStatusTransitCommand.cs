// <copyright file="UserStatusTransitCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.UserStatusTransitCommand
{
    using MediatR;

    /// <summary>
    /// Command object representing a request to transition a user's status.
    /// </summary>
    public class UserStatusTransitCommand : IRequest<UserStatusTransitCommandResponse>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to update.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the target status for the user.
        /// </summary>
        public string? TargetStatus { get; set; }

        /// <summary>
        /// Gets or sets the business reason for the status change.
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Gets or sets the current version number for optimistic locking.
        /// </summary>
        public int? Version { get; set; }
    }
}
