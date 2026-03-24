// <copyright file="ValidateAccountHierarchyCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.ValidateAccountHierarchyCommand
{
    using MediatR;

    /// <summary>
    /// Represents a request to validate the relationship between a head account and a sub account.
    /// </summary>
    public class ValidateAccountHierarchyCommand : IRequest<ValidateAccountHierarchyResponse>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the head account.
        /// </summary>
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the sub account.
        /// </summary>
        public Guid SubAccountId { get; set; }
    }
}
