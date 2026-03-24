// <copyright file="CreateAccountResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Create
{
    using AccountManager.Application.Commands.CreateAccountCommand;
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile that defines mappings for account creation responses.
    /// </summary>
    public class CreateAccountResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountResponseMapping"/> class
        /// and configures mappings between application and API response models.
        /// </summary>
        public CreateAccountResponseMapping()
        {
            CreateMap<CreateAccountCommandResponse, CreateAccountResponse>();
        }
    }
}
