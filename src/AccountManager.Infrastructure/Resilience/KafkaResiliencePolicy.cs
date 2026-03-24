// <copyright file="KafkaResiliencePolicy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Resiliencez
{
    using AccountManager.Shared.Logging;
    using Confluent.Kafka;
    using Polly;

    /// <summary>
    /// Provides Polly-based resilience policies for Kafka producers,
    /// including retry and circuit breaker strategies.
    /// </summary>
    public static class KafkaResiliencePolicy
    {
        /// <summary>
        /// Creates a composed asynchronous resilience policy for Kafka operations.
        /// </summary>
        /// <param name="logger">
        /// The application logger used to record retry and circuit breaker events.
        /// </param>
        /// <returns>
        /// An <see cref="IAsyncPolicy"/> that applies retry and circuit breaker behavior
        /// to Kafka produce operations.
        /// </returns>
        public static IAsyncPolicy Create(IApplogger logger)
        {
            var retryPolicy = Policy
                .Handle<ProduceException<string, string>>()
                .Or<KafkaException>()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: attempt =>
                        TimeSpan.FromMilliseconds(
                            (Math.Pow(2, attempt) * 200) +
                            Random.Shared.Next(0, 100)), // jitter
                    onRetry: (ex, delay, attempt, _) =>
                    {
                        logger.LogError(
                            ex.Message,
                            "Kafka retry {Attempt} after {Delay}",
                            attempt.ToString(),
                            nameof(Create));
                    });

            var circuitBreakerPolicy = Policy
                .Handle<ProduceException<string, string>>()
                .Or<KafkaException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (ex, breakDelay) =>
                    {
                        logger.LogError(
                            ex.Message,
                            "Kafka circuit opened for {BreakDelay}",
                            breakDelay.ToString());
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Kafka circuit closed (recovered).");
                    },
                    onHalfOpen: () =>
                    {
                        logger.LogError("Kafka circuit half-open, testing...");
                    });

            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }
    }
}
