// <copyright file="UnlinkSubAccountResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Unlink_Sub_Account
{
    using AccountManager.Application.Commands.UnlinkSubAccountCommand;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between
    /// <see cref="UnlinkSubAccountCommandResponse"/> and <see cref="UnlinkSubAccountEndpointResponse"/>.
    /// </summary>
    public class UnlinkSubAccountResponseMapping : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnlinkSubAccountResponseMapping"/> class.
        /// Configures the mapping rules for unlink sub-account responses.
        /// </summary>
        public UnlinkSubAccountResponseMapping()
        {
            CreateMap<UnlinkSubAccountCommandResponse, UnlinkSubAccountEndpointResponse>();
        }
    }
}
