// <copyright file="ArchiveAccountResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Archive
{
    using AccountManager.Application.Commands.ArchiveAccountCommand;
    using AutoMapper;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between
    /// application command responses and API response models
    /// related to account archival operations.
    /// </summary>
    public class ArchiveAccountResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveAccountResponseMapping"/> class.
        /// Configures the mapping between <see cref="ArchiveAccountCommandResponse"/> and <see cref="ArchiveAccountEndpointResponse"/>.
        /// </summary>
        public ArchiveAccountResponseMapping()
        {
            CreateMap<ArchiveAccountCommandResponse, ArchiveAccountEndpointResponse>();
        }
    }
}
