// <copyright file="CreateAccountCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.CreateAccountCommand
{
    using AccountManager.Domain.DTO;
    using MediatR;

    /// <summary>
    /// Command used to request the creation of a new account.
    /// </summary>
    public class CreateAccountCommand : IRequest<CreateAccountCommandResponse>
    {
        /// <summary>
        /// Gets or sets the account data required to create a new account.
        /// </summary>
        required public CreateAccountDto Account { get; set; }
    }
}
