// <copyright file="KafkaOutboxWorker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Outbox.Workers
{
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Interfaces;
    using AccountManager.Domain.DTO;
    using AccountManager.Shared.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Polly;
    using Polly.CircuitBreaker;

    /// <summary>
    /// Background worker responsible for processing Kafka outbox messages.
    /// <para>
    /// This worker implements the Outbox Pattern to ensure reliable event publishing
    /// by reading pending events from persistent storage and publishing them to Kafka
    /// with retry, circuit breaker, and dead-letter queue support.
    /// </para>
    /// </summary>
    public class KafkaOutboxWorker : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IApplogger logger;
        private readonly OutboxProcessorOptions options;
        private readonly SemaphoreSlim semaphore;
        private readonly IAsyncPolicy kafkaPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaOutboxWorker"/> class.
        /// </summary>
        /// <param name="scopeFactory">
        /// Factory used to create dependency injection scopes for each processing batch.
        /// </param>
        /// <param name="logger">
        /// Application logger used for structured logging.
        /// </param>
        /// <param name="options">
        /// Configuration options controlling batch size, retry limits, and polling intervals.
        /// </param>
        /// <param name="kafkaPolicy">
        /// Polly resilience policy applied to Kafka publishing operations
        /// (e.g., retries, circuit breaker).
        /// </param>
        public KafkaOutboxWorker(
            IServiceScopeFactory scopeFactory,
            IApplogger logger,
            IOptions<OutboxProcessorOptions> options,
            IAsyncPolicy kafkaPolicy)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
            this.options = options.Value;
            this.kafkaPolicy = kafkaPolicy;
            semaphore = new SemaphoreSlim(this.options.MaxParallelism);
        }

        /// <summary>
        /// Executes the background worker loop.
        /// <para>
        /// Continuously polls for pending outbox messages, processes them in batches,
        /// and respects graceful shutdown signals via the provided cancellation token.
        /// </para>
        /// </summary>
        /// <param name="stoppingToken">
        /// Token that signals when the host is shutting down.
        /// </param>
        /// <returns>A task representing the lifetime of the background service.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation(
                $"Kafka Outbox Worker starting. BatchSize={options.BatchSize}, MaxRetries={options.MaxRetries}, PollingInterval={options.PollingIntervalSeconds}s");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessBatchAsync(stoppingToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    logger.LogInformation("Kafka Outbox Worker stopping gracefully");
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogException(ex, "Unhandled error in Kafka Outbox Worker");
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(options.PollingIntervalSeconds),
                    stoppingToken);
            }

            logger.LogInformation("Kafka Outbox Worker stopped");
        }

        /// <summary>
        /// Processes a single batch of outbox messages.
        /// <para>
        /// Messages are categorized into eligible and dead-letter groups based on retry count.
        /// Eligible messages are published to Kafka with controlled parallelism, while
        /// permanently failed messages are moved to the dead-letter queue.
        /// </para>
        /// </summary>
        /// <param name="ct">
        /// Cancellation token used to cancel batch processing.
        /// </param>
        /// <returns>A task representing the asynchronous batch operation.</returns>
        private async Task ProcessBatchAsync(CancellationToken ct)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

                var messages = await repository.GetPendingKafkaProducedEvents(
                    options.BatchSize,
                    ct);

                if (!messages.Any())
                {
                    return;
                }

                logger.LogInformation("Processing {Count} outbox messages", messages.Count);

                var eligibleMessages = new List<KafkaProducedEventDto>();
                var deadLetterMessages = new List<KafkaProducedEventDto>();

                foreach (var msg in messages)
                {
                    if (msg.RetryCount >= options.MaxRetries)
                    {
                        deadLetterMessages.Add(msg);
                    }
                    else
                    {
                        eligibleMessages.Add(msg);
                    }
                }

                if (deadLetterMessages.Any())
                {
                    try
                    {
                        var eventData = deadLetterMessages.Select(m => (m.AccountId.ToString(), m.Payload ?? string.Empty)).ToList();
                        await publisher.PublishToDeadLetterQueueTopicAsync(eventData, ct);
                        await repository.MarkAsDeadLetterBatchAsync(deadLetterMessages.Select(m => m.Id).ToList(), ct);
                        logger.LogInformation($"Moved {deadLetterMessages.Count} messages to dead letter queue");
                    }
                    catch (Exception ex)
                    {
                        logger.LogException(ex, "Failed while processing dead-letter messages");
                    }
                }

                var tasks = new List<Task<bool>>();
                var results = new List<bool>();

                foreach (var message in eligibleMessages)
                {
                    tasks.Add(ProcessSingleMessageAsync(
                        message,
                        repository,
                        publisher,
                        ct));

                    if (tasks.Count >= options.MaxParallelism)
                    {
                        results.AddRange(await Task.WhenAll(tasks));
                        tasks.Clear();
                    }
                }

                if (tasks.Any())
                {
                    results.AddRange(await Task.WhenAll(tasks));
                }

                var eventIdsToMark = eligibleMessages.Select(msg => msg.Id).ToList();
                await repository.MarkAsProcessedAsync(eventIdsToMark, ct);
                var successCount = results.Count(r => r);

                logger.LogInformation($"Batch complete: {successCount}/{messages.Count} messages processed successfully. DLQ Message Count: {deadLetterMessages.Count}");
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "Error in ProcessBatchAsync");
            }
        }

        /// <summary>
        /// Processes a single outbox message.
        /// <para>
        /// Publishes the message to Kafka using a resilience policy and updates the
        /// outbox state based on the outcome (processed or retry increment).
        /// </para>
        /// </summary>
        /// <param name="message">The outbox message to process.</param>
        /// <param name="repository">Repository used to update outbox state.</param>
        /// <param name="publisher">Kafka event publisher.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if the message was successfully processed; otherwise <c>false</c>.
        /// </returns>
        private async Task<bool> ProcessSingleMessageAsync(
            KafkaProducedEventDto message,
            IOutboxRepository repository,
            IEventPublisher publisher,
            CancellationToken ct)
        {
            await semaphore.WaitAsync(ct);

            try
            {
                await kafkaPolicy.ExecuteAsync(
                    async token =>
                {
                    await publisher.PublishToAccountEventTopicAsync(message.AccountId.ToString(), message.Payload ?? string.Empty);
                }, ct);

                logger.LogInformation(
                    $"Successfully processed message {message.Id} on topic {message.TopicName}");

                return true;
            }
            catch (BrokenCircuitException ex)
            {
                logger.LogException(
                    ex,
                    $"Circuit breaker open for message {message.Id} on topic {message.TopicName}");

                await repository.IncrementRetryAsync(
                    message.Id,
                    CancellationToken.None);

                return false;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                logger.LogInformation($"Processing cancelled for message {message.Id}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogException(
                    ex,
                    $"Failed to process message {message.Id}  on topic  {message.TopicName}");

                await repository.IncrementRetryAsync(
                    message.Id,
                    CancellationToken.None);

                return false;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
