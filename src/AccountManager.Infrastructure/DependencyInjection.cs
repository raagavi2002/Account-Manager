// <copyright file="DependencyInjection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure
{
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Authorization;
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Commands.CreateAccountCommand;
    using AccountManager.Application.Events;
    using AccountManager.Application.Interfaces;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Infrastructure.Authorization;
    using AccountManager.Infrastructure.Authorization.Caching;
    using AccountManager.Infrastructure.Events;
    using AccountManager.Infrastructure.Kafka.Configuration;
    using AccountManager.Infrastructure.Kafka.Producer;
    using AccountManager.Infrastructure.Logging;
    using AccountManager.Infrastructure.Outbox;
    using AccountManager.Infrastructure.Outbox.Workers;
    using AccountManager.Infrastructure.Persistence;
    using AccountManager.Infrastructure.Persistence.Repository;
    using AccountManager.Infrastructure.Persistence.Repository.AuditLog;
    using AccountManager.Infrastructure.Resiliencez;
    using AccountManager.Infrastructure.Services;
    using AccountManager.Shared.Configuration;
    using AccountManager.Shared.Logging;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Serilog;

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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            return services.AddConfiguration(configuration)
                .AddPermissionCaching(configuration)
                .AddAccountManagerDatabase(configuration)
                .AddRepositories()
                .AddCustomLogging(configuration)
                .AddEventInfo(configuration)
                .AddAutoMapperProfiles()
                .AddKafkaOutboxWorker(configuration);
        }

        private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaOptions>(configuration.GetSection("KafkaOptions"));
            services.Configure<OpenSearchSettings>(configuration.GetSection("OpenSearchSettings"));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpClient<IClerkService, ClerkService>();
            return services;
        }

        private static IServiceCollection AddPermissionCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PermissionCacheOptions>(configuration.GetSection("PermissionCache"));

            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            if (!string.IsNullOrWhiteSpace(redisConnectionString))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                    options.InstanceName = "account-manager:";
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            services.AddScoped<ISessionPermissionCache, SessionPermissionCache>();
            return services;
        }

        private static IServiceCollection AddAccountManagerDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AccountManagerDBConnection") ?? throw new InvalidOperationException("AccountManagerDBConnection is required");
            services.AddDbContext<AccountManagerDbContext>(options => options.UseNpgsql(connectionString));
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IOutboxRepository, OutboxRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRelationshipRepository, AccountRelationshipRepository>();
            services.AddScoped<IPermissionCalculator, PermissionCalculator>();
            services.AddScoped<IPermissionResolver, PermissionResolver>();
            services.AddScoped<IPermissionValidator, PermissionValidator>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            return services;
        }

        private static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(TimeProvider.System);

            // Use Serilog Extensions here
            var logger = new LoggerConfiguration()
                .ConfigureSerilog(configuration, services.BuildServiceProvider())
                .CreateLogger();

            Log.Logger = logger;

            services.AddSingleton<Serilog.ILogger>(logger);
            services.AddLogging(builder => builder.ClearProviders().AddSerilog(logger, dispose: true));
            services.AddSingleton<IApplogger, AppLogger>();

            return services;
        }

        private static IServiceCollection AddEventInfo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEventFactory, EventFactory>();
            services.AddScoped<IEventPublisher, KafkaEventPublisher>();
            return services;
        }

        private static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(DependencyInjection).Assembly, typeof(CreateAccountCommandHandler).Assembly);
            return services;
        }

        private static IServiceCollection AddKafkaOutboxWorker(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutboxProcessorOptions>(configuration.GetSection("OutboxProcessorOptions"));
            services.AddSingleton<IAsyncPolicy>(sp =>
            {
                var logger = sp.GetRequiredService<IApplogger>();
                return KafkaResiliencePolicy.Create(logger);
            });
            services.AddHostedService<KafkaOutboxWorker>();
            return services;
        }
    }
}
