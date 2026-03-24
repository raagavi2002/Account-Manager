// <copyright file="LinkSubAccountResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Link_Sub_Account
{
    using AccountManager.Application.Commands.LinkSubAccountCommand;
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile for mapping link sub-account command responses
    /// to API responses.
    /// </summary>
    public class LinkSubAccountResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkSubAccountResponseMapping"/> class.
        /// </summary>
        public LinkSubAccountResponseMapping()
        {
            CreateMap<LinkSubAccountCommandResponse, LinkSubAccountResponse>()
                .ForMember(
                    dest => dest.RelationshipType,
                    opt => opt.MapFrom(src => src.RelationshipType ?? "HEAD_SUB"));
        }
    }
}
