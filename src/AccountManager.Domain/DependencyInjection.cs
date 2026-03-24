// <copyright file="DependencyInjection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain
{
    using AccountManager.Application.Events;
    using AccountManager.Domain.Events.Factories;
    using AccountManager.Domain.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides methods for configuring and managing dependency injection
    /// within the AccountManager application.
    /// </summary>
    /// <remarks>
    /// This class is intended to register services, repositories, and other
    /// dependencies to the application's IoC container.
    /// </remarks>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all infrastructure services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if services or configuration is null.</exception>
        public static IServiceCollection AddDomainLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDomainEvents();
        }

        private static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventFactory, DomainEventFactory>();
            return services;
        }
    }
}
