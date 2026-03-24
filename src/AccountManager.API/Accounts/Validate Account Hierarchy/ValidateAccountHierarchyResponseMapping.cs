// <copyright file="ValidateAccountHierarchyResponseMapping.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.Validate_Account_Hierarchy
{
    using AccountManager.Application.Commands.ValidateAccountHierarchyCommand;
    using AutoMapper;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between
    /// application command responses and API response models
    /// related to account hierarchy validation.
    /// </summary>
    public class ValidateAccountHierarchyResponseMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateAccountHierarchyResponseMapping"/> class.
        /// Configures the mapping between <see cref="ValidateAccountHierarchyResponse"/> and <see cref="ValidateAccountHierarchyEndpointResponse"/>.
        /// </summary>
        public ValidateAccountHierarchyResponseMapping()
        {
            CreateMap<ValidateAccountHierarchyResponse, ValidateAccountHierarchyEndpointResponse>();
        }
    }
}
