// <copyright file="AccountStatusTransitCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AccountManager.Application.Commands.AccountStatusTransitCommand
{
    using AccountManager.Domain.DTO;
    using MediatR;

    /// <summary>
    /// Represents a command to transition an account from one status to another.
    /// </summary>
    public class AccountStatusTransitCommand : IRequest<AccountStatusTransitCommandResponse>
    {
        /// <summary>
        /// Gets or sets the account status transition information including the target status and related data.
        /// </summary>
        required public AccountStatusTransitDto AccountStatusTransitInfo { get; set; }
    }
}
