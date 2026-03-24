// <copyright file="SerilogExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Logging
{
    using AccountManager.Shared.Configuration;
    using Microsoft.Extensions.Configuration;
    using OpenSearch.Net;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Sinks.OpenSearch;

    /// <summary>
    /// Provides extension methods for configuring Serilog logging.
    /// </summary>
    public static class SerilogExtensions
    {
        /// <summary>
        /// Configures Serilog with settings from the provided configuration and service provider.
        /// </summary>
        /// <param name="loggerConfig">The Serilog logger configuration.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="services">The service provider.</param>
        /// <returns>The configured <see cref="LoggerConfiguration"/>.</returns>
        public static LoggerConfiguration ConfigureSerilog(
            this LoggerConfiguration loggerConfig,
            IConfiguration configuration,
            IServiceProvider services)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            var openSearchSettings = configuration.GetSection(nameof(OpenSearchSettings)).Get<OpenSearchSettings>();
            var appConfig = configuration.GetSection(nameof(ServiceConfiguration)).Get<ServiceConfiguration>();

            ArgumentNullException.ThrowIfNull(loggerConfig);
            ArgumentNullException.ThrowIfNull(appConfig);
            ArgumentNullException.ThrowIfNull(openSearchSettings);

            loggerConfig
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("ServiceName", appConfig.ServiceName ?? "Account Manager")
                .Enrich.WithProperty("ServiceVersion", appConfig.InstanceId ?? "1.0.0")
                .WriteTo.Console(formatProvider: System.Globalization.CultureInfo.InvariantCulture)
                .WriteTo.File(
                    "logs/app-.json",
                    rollingInterval: RollingInterval.Day,
                    formatProvider: System.Globalization.CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(openSearchSettings.NodeUrl))
            {
                loggerConfig.WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(openSearchSettings.NodeUrl))
                {
                    IndexFormat = string.IsNullOrEmpty(openSearchSettings.IndexFormat)
                        ? "acm-logs-{0:yyyy.MM.dd}"
                        : openSearchSettings.IndexFormat,
                    AutoRegisterTemplate = true,
                    TypeName = null,
                    ModifyConnectionSettings = conn => conn
                        .DisableDirectStreaming()
                        .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                        .BasicAuthentication(openSearchSettings.UserName, openSearchSettings.Password)
                        .RequestTimeout(TimeSpan.FromSeconds(30))
                        .MaximumRetries(3),
                    BatchPostingLimit = openSearchSettings.BatchPostingLimit,
                    Period = TimeSpan.FromSeconds(openSearchSettings.Period),
                    EmitEventFailure = Serilog.Sinks.OpenSearch.EmitEventFailureHandling.WriteToSelfLog | Serilog.Sinks.OpenSearch.EmitEventFailureHandling.RaiseCallback,
                    FailureCallback = e => Console.WriteLine($"OpenSearch logging failed: {e.Exception?.Message}"),
                    QueueSizeLimit = 100000,
                    MinimumLogEventLevel = LogEventLevel.Information,
                });
            }

            return loggerConfig;
        }
    }
}
