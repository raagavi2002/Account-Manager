// <copyright file="ArchiveAccountCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.ArchiveAccountCommand
{
    using AccountManager.Domain.DTO;
    using MediatR;

    /// <summary>
    /// Command to request the archival of an account.
    /// Wraps the <see cref="ArchiveAccountDto"/> and is handled via MediatR.
    /// </summary>
    public class ArchiveAccountCommand : IRequest<ArchiveAccountCommandResponse>
    {
        /// <summary>
        /// Gets or sets the data transfer object containing
        /// the details of the account archival request.
        /// </summary>
        required public ArchiveAccountDto ArchiveAccountDto { get; set; }
    }
}
