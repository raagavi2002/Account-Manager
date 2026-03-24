// <copyright file="UpdateAccountResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Update
{
    using AccountManager.Application.Commands.UpdateAccountCommand;
    using AutoMapper;

    /// <summary>
    /// Defines AutoMapper configuration for mapping account update command responses
    /// to API response models.
    /// </summary>
    public class UpdateAccountResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAccountResponseMapping"/> class
        /// and configures mappings related to account update operations.
        /// </summary>
        public UpdateAccountResponseMapping()
        {
           CreateMap<UpdateAccountCommandResponse, UpdateAccountResponse>();
        }
    }
}
