// <copyright file="UpdateAccountCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.UpdateAccountCommand
{
    using AccountManager.Domain.DTO;
    using MediatR;

    /// <summary>
    /// Represents a command used to update an existing account.
    /// </summary>
    /// <remarks>
    /// This command is handled via MediatR and contains the data required
    /// to perform an account update operation.
    /// </remarks>
    public class UpdateAccountCommand : IRequest<UpdateAccountCommandResponse>
    {
        /// <summary>
        /// Gets or sets the data transfer object containing account update information.
        /// </summary>
        required public UpdateAccountDto UpdateAccountDto { get; set; }
    }
}
