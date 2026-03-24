// <copyright file="StatusTransitRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Status_Transit
{
    using AccountManager.Application.Commands.AccountStatusTransitCommand;
    using AccountManager.Domain.DTO;
    using AutoMapper;

    /// <summary>
    /// Defines AutoMapper configuration for mapping
    /// <see cref="StatusTransitRequest"/> to domain DTOs.
    /// </summary>
    /// <remarks>
    /// This mapping is used when handling account status transition requests
    /// and converting API-layer models into application-layer data transfer objects.
    /// </remarks>
    public class StatusTransitRequestMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusTransitRequestMapping"/> class
        /// and configures request-to-DTO mappings.
        /// </summary>
        public StatusTransitRequestMapping()
        {
            CreateMap<StatusTransitRequest, AccountStatusTransitDto>();
        }
    }
}
