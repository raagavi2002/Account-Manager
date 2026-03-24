// <copyright file="StatusTransitResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Status_Transit
{
    using AccountManager.Application.Commands.AccountStatusTransitCommand;
    using AutoMapper;

    /// <summary>
    /// Defines AutoMapper configuration for mapping application command responses
    /// to API response models.
    /// </summary>
    /// <remarks>
    /// This mapping is used to convert the result of an account status transition
    /// command into a response object returned by the API.
    /// </remarks>
    public class StatusTransitResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusTransitResponseMapping"/> class
        /// and configures response mappings.
        /// </summary>
        public StatusTransitResponseMapping()
        {
            CreateMap<AccountStatusTransitCommandResponse, StatusTransitResponse>();
        }
    }
}
