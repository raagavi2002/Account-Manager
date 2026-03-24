// <copyright file="ArchiveAccountCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.ArchiveAccountCommand
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="ArchiveAccountCommand"/> request by archiving an account,
    /// updating its status, anonymizing user data, and publishing domain events.
    /// </summary>
    public class ArchiveAccountCommandHandler : IRequestHandler<ArchiveAccountCommand, ArchiveAccountCommandResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IAccountRelationshipRepository accountRelationshipRepository;
        private readonly IOutboxRepository outboxRepository;
        private readonly IDomainEventFactory domainEventFactory;
        private readonly IApplogger applogger;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveAccountCommandHandler"/> class.
        /// </summary>
        /// <param name="accountRepository">Repository for account data operations.</param>
        /// <param name="accountRelationshipRepository">Repository for account relationship operations.</param>
        /// <param name="outboxRepository">Repository for persisting outbox events.</param>
        /// <param name="domainEventFactory">Factory for creating domain events.</param>
        /// <param name="applogger">Application logger for logging operations.</param>
        /// <param name="unitOfWork">Unit of work for transactional consistency.</param>
        public ArchiveAccountCommandHandler(
            IAccountRepository accountRepository,
            IAccountRelationshipRepository accountRelationshipRepository,
            IOutboxRepository outboxRepository,
            IDomainEventFactory domainEventFactory,
            IApplogger applogger,
            IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.accountRelationshipRepository = accountRelationshipRepository;
            this.outboxRepository = outboxRepository;
            this.domainEventFactory = domainEventFactory;
            this.applogger = applogger;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles the archive account command by validating account existence,
        /// checking archival status, updating account details, and publishing events.
        /// </summary>
        /// <param name="request">The archive account command request.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="ArchiveAccountCommandResponse"/> containing the archival result and metadata.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// Thrown when the account does not exist.
        /// </exception>
        /// <exception cref="AccountAlreadyArchivedException">
        /// Thrown when the account is already archived.
        /// </exception>
        public async Task<ArchiveAccountCommandResponse> Handle(ArchiveAccountCommand request, CancellationToken cancellationToken)
        {
            var accountInfo = await accountRepository.GetAccountInfoByIdAsync(request.ArchiveAccountDto.AccountId).ConfigureAwait(false);

            if (accountInfo == null)
            {
                throw new AccountNotFoundException(new Domain.Errors.ErrorResponses
                {
                    Message = "Account does not exists",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                    Details = new Domain.Errors.ErrorInfo
                    {
                        AccountId = request.ArchiveAccountDto.AccountId,
                    },
                });
            }

            if (string.Equals(accountInfo?.AccountStatus?.ToUpper(), EnumParser.GetEnumMemberValue<AccountStatus>(AccountStatus.Archive)))
            {
                throw new AccountAlreadyArchivedException(new Domain.Errors.ErrorResponses
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                    Message = "Account has been already archived",
                    TimeStamp = DateTime.UtcNow.ToString(),
                    Details = new Domain.Errors.ErrorInfo
                    {
                        AccountId = request?.ArchiveAccountDto.AccountId,
                    },
                });
            }

            var accStatus = EnumParser.GetEnumMemberValue<AccountStatus>(AccountStatus.Archive);
            await accountRepository.UpdateAccountStatusAsync(request.ArchiveAccountDto.AccountId, accStatus, true);

            if (request.ArchiveAccountDto.IsGdprRequest)
            {
                UpdateAccountDto updateAccountDto = new UpdateAccountDto
                {
                    AccountId = request.ArchiveAccountDto.AccountId,
                    AccountName = "Anonymous User",
                };
                await accountRepository.UpdateAccountAsync(updateAccountDto).ConfigureAwait(false);
            }

            AccountStatusTransitDto accountStatusTransitDto = new AccountStatusTransitDto
            {
                AccountId = request.ArchiveAccountDto.AccountId,
                AccountStatus = accStatus,
                Reason = request.ArchiveAccountDto.Reason,
                Version = accountInfo.Version++,
            };

            var statusTransitEvent = domainEventFactory.CreateAccountStatusChangedEvent(accountStatusTransitDto, accountInfo);

            KafkaProducedEventDto kafkaProducedEventDto = new KafkaProducedEventDto
            {
                AccountId = request.ArchiveAccountDto.AccountId,
                EventType = EventTypes.AccountStatusChanged,
                ProducerService = "AccountManager",
                Payload = JsonSerializer.Serialize(statusTransitEvent),
                Status = accStatus,
                RetryCount = 0,
            };

            await outboxRepository.AddKafkaProducedEventAsync(kafkaProducedEventDto, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ArchiveAccountCommandResponse
            {
                AccountId = request.ArchiveAccountDto.AccountId,
                Reason = request?.ArchiveAccountDto.Reason,
                IsArchived = true,
                IsGDPRComplaint = request.ArchiveAccountDto.IsGdprRequest,
                ArchivedAt = DateTime.UtcNow,
                ArchivedBy = "system",
            };
        }
    }
}
