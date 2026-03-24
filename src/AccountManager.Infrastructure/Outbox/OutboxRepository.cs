// <copyright file="OutboxRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Outbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Infrastructure.Kafka.Configuration;
    using AccountManager.Infrastructure.Persistence;
    using AccountManager.Infrastructure.Persistence.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Provides an Entity Framework Core–based implementation of the outbox repository
    /// used to persist and manage Kafka-produced events.
    /// </summary>
    public class OutboxRepository(
        AccountManagerDbContext dbContext,
        IOptions<KafkaOptions> options) : IOutboxRepository
    {
        /// <summary>
        /// The Kafka topic used for publishing account-related events.
        /// </summary>
        private readonly string accountEventsTopic = options.Value.ProducerOptions.AccountEventsTopic;

        /// <summary>
        /// Retrieves a batch of pending Kafka-produced events ordered by creation time.
        /// </summary>
        /// <param name="maxEvents">The maximum number of events to retrieve.</param>
        /// <param name="cancellationToken">A token used to cancel the operation.</param>
        /// <returns>
        /// A read-only list of pending Kafka-produced event data transfer objects.
        /// </returns>
        public async Task<IReadOnlyList<KafkaProducedEventDto>> GetPendingKafkaProducedEvents(
            int maxEvents,
            CancellationToken cancellationToken)
        {
            return await dbContext.KafkaProducedEvents
                .AsNoTracking()
                .Where(e => e.Status == EnumParser.GetEnumMemberValue(OutboxStatus.Pending))
                .OrderBy(e => e.ProducedAt)
                .Take(maxEvents)
                .Select(e => new KafkaProducedEventDto
                {
                    Id = (int)e.Id,
                    TopicName = e.TopicName,
                    EventType = e.EventType,
                    ProducerService = e.ProducerService,
                    Payload = e.Payload,
                    ProducedAt = e.ProducedAt,
                    CorrelationId = e.CorrelationId,
                    Status = e.Status,
                    ErrorMessage = e.ErrorMessage,
                    RetryCount = e.RetryCount,
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Marks the specified Kafka-produced events as dead-lettered after exceeding
        /// the maximum number of retry attempts.
        /// </summary>
        /// <param name="eventIds">The identifiers of the events to update.</param>
        /// <param name="cancellationToken">A token used to cancel the operation.</param>
        /// <returns>representing the asynchronous operation.</returns>
        public async Task MarkAsDeadLetterBatchAsync(
            List<long> eventIds,
            CancellationToken cancellationToken)
        {
            await dbContext.KafkaProducedEvents
                .Where(e => eventIds.Contains(e.Id))
                .ForEachAsync(
                    e =>
                    {
                        e.Status = EnumParser.GetEnumMemberValue(OutboxStatus.Failed);
                        e.ErrorMessage = "Exceeded maximum retry attempts";
                    },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Marks the specified Kafka-produced events as successfully processed
        /// and published to Kafka.
        /// </summary>
        /// <param name="eventIds">The identifiers of the events to update.</param>
        /// <param name="cancellationToken">A token used to cancel the operation.</param>
        /// <returns>representing the asynchronous operation.</returns>
        public async Task MarkAsProcessedAsync(
            List<long> eventIds,
            CancellationToken cancellationToken)
        {
            await dbContext.KafkaProducedEvents
                .Where(e => eventIds.Contains(e.Id))
                .ForEachAsync(
                    e =>
                    {
                        e.Status = EnumParser.GetEnumMemberValue(OutboxStatus.Published);
                        e.ErrorMessage = "Event has been published to Kafka";
                    },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Increments the retry count for the specified Kafka-produced event and
        /// updates the error message to indicate a retry attempt.
        /// </summary>
        /// <param name="eventId">The identifier of the event to update.</param>
        /// <param name="cancellationToken">A token used to cancel the operation.</param>
        /// <returns>representing the asynchronous operation.</returns>
        public async Task IncrementRetryAsync(
            long eventId,
            CancellationToken cancellationToken)
        {
            await dbContext.KafkaProducedEvents
                .Where(e => e.Id == eventId)
                .ForEachAsync(
                    e =>
                    {
                        e.RetryCount += 1;
                        e.ErrorMessage = "Retrying to publish event to Kafka";
                    },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a new Kafka-produced event to the outbox for later publishing.
        /// </summary>
        /// <param name="kafkaProducedEventDto">
        /// The Kafka-produced event data to persist.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the operation.
        /// </param>
        /// <returns>representing the asynchronous operation.</returns>
        public async Task AddKafkaProducedEventAsync(
            KafkaProducedEventDto kafkaProducedEventDto,
            CancellationToken cancellationToken)
        {
            KafkaProducedEvent kafkaProducedEvent = new KafkaProducedEvent
            {
                AccountId = kafkaProducedEventDto.AccountId,
                Payload = kafkaProducedEventDto.Payload,
                CorrelationId = kafkaProducedEventDto.CorrelationId,
                Status = kafkaProducedEventDto.Status,
                RetryCount = 0,
                EventType = kafkaProducedEventDto.EventType,
                TopicName = accountEventsTopic,
                ProducedAt = DateTime.UtcNow,
            };

            await dbContext.KafkaProducedEvents
                .AddAsync(kafkaProducedEvent, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
