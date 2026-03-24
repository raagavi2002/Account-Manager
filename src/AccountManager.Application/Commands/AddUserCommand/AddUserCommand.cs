// <copyright file="AddUserCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.AddUserCommand
{
    using AccountManager.Domain.DTO;
    using MediatR;

    /// <summary>
    /// Command to add a new user to an account.
    /// </summary>
    public class AddUserCommand : IRequest<AddUserCommandResponse>
    {
        /// <summary>
        /// Gets or sets the data required to add a new user.
        /// </summary>
        public AddUserDto AddUser { get; set; } = null!;
    }
}
