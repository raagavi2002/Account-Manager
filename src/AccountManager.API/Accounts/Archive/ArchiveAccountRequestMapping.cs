// <copyright file="ArchiveAccountRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Archive
{
    using AccountManager.Domain.DTO;
    using AutoMapper;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between
    /// API request models and domain data transfer objects (DTOs)
    /// related to account archival operations.
    /// </summary>
    public class ArchiveAccountRequestMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveAccountRequestMapping"/> class.
        /// Configures the mapping between <see cref="ArchiveAccountEndpointRequest"/> and <see cref="ArchiveAccountDto"/>.
        /// </summary>
        public ArchiveAccountRequestMapping()
        {
            CreateMap<ArchiveAccountEndpointRequest, ArchiveAccountDto>();
        }
    }
}
