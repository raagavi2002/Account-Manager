// <copyright file="AddUserEndpointResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.Add_User
{
    using AccountManager.Application.Commands.AddUserCommand;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between
    /// <see cref="AddUserCommandResponse"/> and <see cref="AddUserEndpointResponse"/>.
    /// </summary>
    public class AddUserEndpointResponseMapping : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserEndpointResponseMapping"/> class.
        /// Configures the mapping rules between the command response and the API endpoint response.
        /// </summary>
        public AddUserEndpointResponseMapping()
        {
            // Maps domain command response data to the API response model for returning to the client.
            CreateMap<AddUserCommandResponse, AddUserEndpointResponse>();
        }
    }
}
