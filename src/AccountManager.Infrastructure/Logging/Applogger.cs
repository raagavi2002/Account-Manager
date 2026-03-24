// <copyright file="Applogger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Logging
{
    using System.Buffers;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using AccountManager.Shared.Configuration;
    using AccountManager.Shared.Logging;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Provides structured, high-performance, and context-rich logging across all application layers.
    /// </summary>
    public sealed partial class AppLogger : IApplogger
    {
        private readonly ILogger<AppLogger> logger;
        private readonly string serviceName;
        private readonly string serviceVersion;
        private readonly string environment;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLogger"/> class.
        /// </summary>
        /// <param name="applogger">The logger instance used for logging operations.</param>
        /// <param name="appConfigOptions">Application configuration options.</param>
        /// <param name="timeprovider">Time provider used for timestamp generation.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="applogger"/> or <paramref name="appConfigOptions"/> is null.
        /// </exception>
        public AppLogger(ILogger<AppLogger> applogger, IOptions<AppConfig> appConfigOptions, TimeProvider timeprovider)
        {
            ArgumentNullException.ThrowIfNull(applogger);
            ArgumentNullException.ThrowIfNull(appConfigOptions);

            logger = applogger;

            var appConfig = appConfigOptions.Value;
            serviceName = appConfig.ServiceName ?? "UnknownService";
            serviceVersion = appConfig.ServiceVersion ?? "1.0.0";
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            timeProvider = timeprovider;
        }

        /// <summary>
        /// Logs an exception with contextual metadata.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">A message describing the context of the exception.</param>
        /// <param name="additionalInfo">Optional additional diagnostic data.</param>
        /// <param name="filePath">Caller file path (compiler supplied).</param>
        /// <param name="methodName">Caller member name (compiler supplied).</param>
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public void LogException(Exception ex, string message, object? additionalInfo = null, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "")
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            var metadata = CreateMetadata(filePath, methodName);
            var exceptionDetails = BuildOptimizedExceptionString(ex);
            LogExceptionInternal(logger, message, ex, metadata.ServiceName, metadata.ServiceVersion, metadata.Environment, metadata.ProjectLayer, metadata.ClassName, metadata.MethodName, exceptionDetails);
        }

        /// <summary>
        /// Logs an error message with contextual metadata.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="additionalInfo">Optional additional diagnostic data.</param>
        /// <param name="filePath">Caller file path (compiler supplied).</param>
        /// <param name="methodName">Caller member name (compiler supplied).</param>
        public void LogError(string message, object? additionalInfo = null, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "")
        {
            var metadata = CreateMetadata(filePath, methodName);
            LogErrorInternal(logger, message, metadata.ServiceName, metadata.ServiceVersion, metadata.Environment, metadata.ProjectLayer, metadata.ClassName, metadata.MethodName);
        }

        /// <summary>
        /// Logs an informational message with contextual metadata.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        /// <param name="additionalInfo">Optional additional diagnostic data.</param>
        /// <param name="filePath">Caller file path (compiler supplied).</param>
        /// <param name="methodName">Caller member name (compiler supplied).</param>
        public void LogInformation(string message, object? additionalInfo = null, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "")
        {
            var metadata = CreateMetadata(filePath, methodName);
            LogInformationInternal(logger, message, metadata.ServiceName, metadata.ServiceVersion, metadata.Environment, metadata.ProjectLayer, metadata.ClassName, metadata.MethodName);
        }

        /// <summary>
        /// Logs a warning message with contextual metadata.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        /// <param name="additionalInfo">Optional additional diagnostic data.</param>
        /// <param name="filePath">Caller file path (compiler supplied).</param>
        /// <param name="methodName">Caller member name (compiler supplied).</param>
        public void LogWarning(string message, object? additionalInfo = null, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "")
        {
            var metadata = CreateMetadata(filePath, methodName);
            LogWarningInternal(logger, message, metadata.ServiceName, metadata.ServiceVersion, metadata.Environment, metadata.ProjectLayer, metadata.ClassName, metadata.MethodName);
        }

        /// <summary>
        /// Begins a logical operation scope for structured logging.
        /// </summary>
        /// <typeparam name="TState">The type of the scope state.</typeparam>
        /// <param name="state">The scope state.</param>
        /// <returns>An <see cref="IDisposable"/> that ends the scope.</returns>
        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull
        {
            return logger.BeginScope(state) !;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<char> ExtractProjectLayerFromPath(ReadOnlySpan<char> filePath)
        {
            var segments = filePath.ToString().Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            foreach (var segment in segments)
            {
                if (segment.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                {
                    return "Presentation";
                }

                if (segment.EndsWith("Service", StringComparison.OrdinalIgnoreCase))
                {
                    return "Application";
                }

                if (segment.EndsWith("Repository", StringComparison.OrdinalIgnoreCase))
                {
                    return "Infrastructure";
                }

                if (segment.Contains("Domain", StringComparison.OrdinalIgnoreCase))
                {
                    return "Domain";
                }
            }

            return "Unknown";
        }

        private static string BuildOptimizedExceptionString(Exception ex)
        {
            var rentedBuffer = ArrayPool<char>.Shared.Rent(4096);

            try
            {
                var sb = new StringBuilder();
                BuildExceptionDetails(ex, sb, 0, 5);
                return sb.ToString();
            }
            finally
            {
                ArrayPool<char>.Shared.Return(rentedBuffer);
            }
        }

        private static void BuildExceptionDetails(Exception ex, StringBuilder sb, int currentDepth, int maxDepth)
        {
            if (currentDepth >= maxDepth)
            {
                sb.AppendLine("Maximum exception depth reached...");
                return;
            }

            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "[Depth: {0}] {1}: {2}", currentDepth, ex.GetType().Name, ex.Message));

            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                var stackLines = ex.StackTrace.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                var linesToTake = Math.Min(stackLines.Length, 5);

                for (int i = 0; i < linesToTake; i++)
                {
                    sb.AppendLine(stackLines[i].Trim());
                }
            }

            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception:");
                BuildExceptionDetails(ex.InnerException, sb, currentDepth + 1, maxDepth);
            }

            if (ex is AggregateException aggEx)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Aggregate Exception with {0} inner exceptions:", aggEx.InnerExceptions.Count));

                for (int i = 0; i < Math.Min(aggEx.InnerExceptions.Count, 3); i++)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Inner Exception {0}:", i + 1));
                    BuildExceptionDetails(aggEx.InnerExceptions[i], sb, currentDepth + 1, maxDepth);
                }
            }
        }

#pragma warning disable S3251
        [LoggerMessage(EventId = 1001, Level = LogLevel.Error, Message = "Exception in {ServiceName}:{ServiceVersion} [{Environment}] {ProjectLayer}.{ClassName}.{MethodName} - {Message} | Details: {ExceptionDetails}")]
        static partial void LogExceptionInternal(ILogger logger, string message, Exception exception, string serviceName, string serviceVersion, string environment, string projectLayer, string className, string methodName, string exceptionDetails);

        [LoggerMessage(EventId = 1002, Level = LogLevel.Error, Message = "Error in {ServiceName}:{ServiceVersion} [{Environment}] {ProjectLayer}.{ClassName}.{MethodName} - {Message}")]
        static partial void LogErrorInternal(ILogger logger, string message, string serviceName, string serviceVersion, string environment, string projectLayer, string className, string methodName);

        [LoggerMessage(EventId = 1003, Level = LogLevel.Information, Message = "Info in {ServiceName}:{ServiceVersion} [{Environment}] {ProjectLayer}.{ClassName}.{MethodName} - {Message}")]
        static partial void LogInformationInternal(ILogger logger, string message, string serviceName, string serviceVersion, string environment, string projectLayer, string className, string methodName);

        [LoggerMessage(EventId = 1004, Level = LogLevel.Warning, Message = "Warning in {ServiceName}:{ServiceVersion} [{Environment}] {ProjectLayer}.{ClassName}.{MethodName} - {Message}")]
        static partial void LogWarningInternal(ILogger logger, string message, string serviceName, string serviceVersion, string environment, string projectLayer, string className, string methodName);
#pragma warning restore S3251

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private LogMetaData CreateMetadata(string filePath, string methodName)
        {
            ReadOnlySpan<char> fileName = Path.GetFileNameWithoutExtension(filePath.AsSpan());
            ReadOnlySpan<char> projectLayer = ExtractProjectLayerFromPath(filePath.AsSpan());

            return new LogMetaData
            {
                ServiceName = serviceName,
                ServiceVersion = serviceVersion,
                Environment = environment,
                ProjectLayer = projectLayer.ToString(),
                ClassName = fileName.ToString(),
                MethodName = methodName,
                Timestamp = timeProvider.GetUtcNow(),
                CorrelationId = Activity.Current?.Id,
            };
        }
    }
}
