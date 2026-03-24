// <copyright file="UpdateAccountRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Update
{
    using AccountManager.Domain.DTO;
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile for mapping <see cref="UpdateAccountDto"/> to <see cref="UpdateAccountRequest"/>.
    /// </summary>
    public class UpdateAccountRequestMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAccountRequestMapping"/> class.
        /// Configures the mapping between <see cref="UpdateAccountDto"/> and <see cref="UpdateAccountRequest"/>.
        /// </summary>
        public UpdateAccountRequestMapping()
        {
            CreateMap<UpdateAccountDto, UpdateAccountRequest>();
        }
    }
}
