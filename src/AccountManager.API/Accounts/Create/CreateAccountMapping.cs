// <copyright file="CreateAccountMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Create
{
    using AccountManager.Domain.DTO;
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile responsible for mapping create account
    /// API request models to domain DTOs.
    /// </summary>
    public class CreateAccountMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountMapping"/> class
        /// and configures mapping definitions for account creation.
        /// </summary>
        public CreateAccountMapping()
        {
            CreateMap<CreateAddressRequest, AddressDto>();
            CreateMap<CreateAccountRequest, CreateAccountDto>()
                .ForMember(
                    dest => dest.Address,
                    opt => opt.MapFrom(src => src.AddressInfo));
        }
    }
}
