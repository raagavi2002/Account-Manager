// <copyright file="LinkSubAccountCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.LinkSubAccountCommand
{
    using AccountManager.Domain.DTO;
    using MediatR;

    /// <summary>
    /// Represents a command to link a sub-account to a head account.
    /// </summary>
    /// <remarks>
    /// This command is handled by the application layer to establish a
    /// head–sub account relationship between two existing accounts.
    /// The relationship details are provided via a
    /// <see cref="LinkSubAccountDto"/> instance.
    /// </remarks>
    public class LinkSubAccountCommand : IRequest<LinkSubAccountCommandResponse>
    {
        /// <summary>
        /// Gets or sets the data transfer object containing
        /// information required to link a sub-account to a head account.
        /// </summary>
        /// <remarks>
        /// This DTO includes identifiers for both the head account and
        /// the sub-account, as well as the type of relationship to be created.
        /// </remarks>
        required public LinkSubAccountDto LinkSubAccountDto { get; set; }
    }
}
