// <copyright file="UnlinkSubAccountRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Unlink_Sub_Account
{
    using AccountManager.Domain.DTO;
    using AutoMapper;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between
    /// <see cref="UnlinkSubAccountEndpointRequest"/> and <see cref="UnlinkSubAccountDto"/>.
    /// </summary>
    public class UnlinkSubAccountRequestMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnlinkSubAccountRequestMapping"/> class.
        /// Configures the mapping rules for unlinking a sub-account request.
        /// </summary>
        public UnlinkSubAccountRequestMapping()
        {
            CreateMap<UnlinkSubAccountEndpointRequest, UnlinkSubAccountDto>()
                .ForMember(
                    dest => dest.HeadAccountId,
                    opt => opt.MapFrom(src => src.HeadAccountId))
                .ForMember(
                    dest => dest.SubAccountId,
                    opt => opt.MapFrom(src => src.SubAccountId))
                .ForMember(
                    dest => dest.Reason,
                    opt => opt.MapFrom(src => src.Reason));
        }
    }
}
