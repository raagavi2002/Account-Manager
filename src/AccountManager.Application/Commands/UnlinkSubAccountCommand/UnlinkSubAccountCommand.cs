// <copyright file="UnlinkSubAccountCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.UnlinkSubAccountCommand
{
    using AccountManager.Domain.DTO;
    using MediatR;

    /// <summary>
    /// Represents a command to unlink a sub-account from a parent account.
    /// </summary>
    /// <remarks>
    /// This command is handled by MediatR and returns an <see cref="UnlinkSubAccountCommandResponse"/>.
    /// </remarks>
    public class UnlinkSubAccountCommand : IRequest<UnlinkSubAccountCommandResponse>
    {
        /// <summary>
        /// Gets or sets the information required to unlink the sub-account.
        /// </summary>
        /// <value>
        /// A <see cref="LinkSubAccountDto"/> containing details of the sub-account to be unlinked.
        /// </value>
        required public UnlinkSubAccountDto UnlinkAccountInfo { get; set; }
    }
}
