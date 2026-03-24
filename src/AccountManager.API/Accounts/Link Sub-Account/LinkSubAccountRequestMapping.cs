// <copyright file="LinkSubAccountRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Link_Sub_Account
{
    using AccountManager.Domain.DTO;
    using AutoMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkSubAccountRequestMapping"/> class
    /// and configures mapping definitions for account linking.
    /// </summary>
    public class LinkSubAccountRequestMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkSubAccountRequestMapping"/> class
        /// and configures mappings related to account relationships.
        /// </summary>
        public LinkSubAccountRequestMapping()
        {
            CreateMap<LinkSubAccountRequest, LinkSubAccountDto>()
            .ForMember(
                dest => dest.HeadAccountId,
                opt => opt.MapFrom(src => src.HeadAccountId))
            .ForMember(
                dest => dest.SubAccountId,
                opt => opt.MapFrom(src => src.SubAccountId))
            .ForMember(
                dest => dest.RelationshipType,
                opt => opt.MapFrom(src => src.RelationshipType));
        }
    }
}
