// <copyright file="ServiceRegistrationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Extensions
{
    using AccountManager.API.Accounts.Archive;
    using AccountManager.API.Accounts.Create;
    using AccountManager.API.Accounts.GetAccountDetails;
    using AccountManager.API.Accounts.GetAccountProducts;
    using AccountManager.API.Accounts.Link_Sub_Account;
    using AccountManager.API.Accounts.ListAccountUsers;
    using AccountManager.API.Accounts.Status_Transit;
    using AccountManager.API.Accounts.Unlink_Sub_Account;
    using AccountManager.API.Accounts.Update;
    using AccountManager.API.Accounts.Validate_Account_Hierarchy;
    using AccountManager.API.User.Add_User;
    using AccountManager.API.User.GetUser;
    using AccountManager.API.User.UpdateUser;
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Commands.AccountStatusTransitCommand;
    using AccountManager.Application.Commands.ArchiveAccountCommand;
    using AccountManager.Application.Commands.CreateAccountCommand;
    using AccountManager.Application.Commands.CreateAuditEntryCommand;
    using AccountManager.Application.Commands.LinkSubAccountCommand;
    using AccountManager.Application.Commands.UnlinkSubAccountCommand;
    using AccountManager.Application.Commands.UpdateAccountCommand;
    using AccountManager.Application.Commands.ValidateAccountHierarchyCommand;
    using AccountManager.Application.Queries.GetAccountDetailsQuery;
    using AccountManager.Application.Queries.GetAccountProductsQuery;
    using AccountManager.Application.Queries.GetTimezoneQuery;
    using AccountManager.Application.Queries.GetUserQuery;
    using AccountManager.Application.Commands.UpdateUserCommand;
    using AccountManager.Infrastructure.Authorization;
    using ClerkShared.Auth;

    /// <summary>
    /// Provides extension methods for registering application services such as MediatR and AutoMapper.
    /// </summary>
    public static class ServiceRegistrationExtensions
    {
        /// <summary>
        /// Registers MediatR and AutoMapper profiles for the application.
        /// </summary>
        /// <param name="services">The service collection to add registrations to.</param>
        /// <returns>DI.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // MediatR registrations
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateAccountCommandHandler>();
                cfg.RegisterServicesFromAssemblyContaining<AccountStatusTransitCommandHandler>();
                cfg.RegisterServicesFromAssemblyContaining<LinkSubAccountCommandHandler>();
                cfg.RegisterServicesFromAssemblyContaining<UpdateAccountCommandHandler>();
                cfg.RegisterServicesFromAssemblyContaining<GetAccountDetailsQueryHandler>();
                cfg.RegisterServicesFromAssemblyContaining<GetAccountProductsQueryHandler>();
                cfg.RegisterServicesFromAssemblyContaining<GetTimezoneQueryHandler>();
                cfg.RegisterServicesFromAssemblyContaining<UnlinkSubAccountCommandHandler>();
                cfg.RegisterServicesFromAssemblyContaining<ArchiveAccountCommandHandler>();
                cfg.RegisterServicesFromAssemblyContaining<ValidateAccountHierarchyCommandHandler>();
                cfg.RegisterServicesFromAssemblyContaining<GetAccountProductsQueryHandler>();
                cfg.RegisterServicesFromAssemblyContaining<CreateAuditEntryCommandHandler>();
                cfg.RegisterServicesFromAssemblyContaining<GetUserQueryHandler>();
                cfg.RegisterServicesFromAssemblyContaining<UpdateUserCommandHandler>();
            });

            // AutoMapper registrations
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CreateAccountMapping>();
                cfg.AddProfile<CreateAccountResponseMapping>();
                cfg.AddProfile<StatusTransitRequestMapping>();
                cfg.AddProfile<StatusTransitResponseMapping>();
                cfg.AddProfile<LinkSubAccountRequestMapping>();
                cfg.AddProfile<LinkSubAccountResponseMapping>();
                cfg.AddProfile<UpdateAccountRequestMapping>();
                cfg.AddProfile<UpdateAccountResponseMapping>();
                cfg.AddProfile<GetAccountDetailsAPIRequestMapping>();
                cfg.AddProfile<GetAccountDetailsResponseMapping>();
                cfg.AddProfile<GetAccountProductsAPIRequestMapping>();
                cfg.AddProfile<GetAccountProductsResponseMapping>();
                cfg.AddProfile<AddUserEndpointRequestMapping>();
                cfg.AddProfile<AddUserEndpointResponseMapping>();
                cfg.AddProfile<UnlinkSubAccountRequestMapping>();
                cfg.AddProfile<UnlinkSubAccountResponseMapping>();
                cfg.AddProfile<ArchiveAccountRequestMapping>();
                cfg.AddProfile<ArchiveAccountResponseMapping>();
                cfg.AddProfile<ValidateAccountHierarchyResponseMapping>();
                cfg.AddProfile<ListAccountUsersResponseMapping>();
                cfg.AddProfile<ListAccountUsersAPIRequestMapping>();
                cfg.AddProfile<GetUserEndpointResponseMapping>();
                cfg.AddProfile<UpdateUserEndpointResponseMapping>();
            });

            /*            services.AddScoped<IPermissionResolver, PermissionResolver>();*/
            services.AddClerkAuth(
                issuer: "https://driven-starfish-28.clerk.accounts.dev");
            return services;
        }
    }
}
