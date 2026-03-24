// <copyright file="AddUserEndpointRequestMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.User.Add_User
{
    using AccountManager.Domain.DTO;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between
    /// <see cref="AddUserEndpointRequest"/> and <see cref="AddUserDto"/>.
    /// </summary>
    public class AddUserEndpointRequestMapping : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserEndpointRequestMapping"/> class.
        /// Configures the mapping rules between endpoint request and DTO.
        /// </summary>
        public AddUserEndpointRequestMapping()
        {
            // Maps incoming API request data to the domain DTO for further processing.
            CreateMap<AddUserEndpointRequest, AddUserDto>();
        }
    }
}
