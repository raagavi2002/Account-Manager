// <copyright file="UnlinkSubAccountCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.UnlinkSubAccountCommand
{
    using AccountManager.Application.Abstractions;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using MediatR;

    /// <summary>
    /// Handles the unlinking of a sub-account from a head account.
    /// </summary>
    /// <remarks>
    /// This command handler validates account existence, ensures a relationship exists,
    /// unlinks the sub-account, persists changes, and produces a domain event.
    /// </remarks>
    public class UnlinkSubAccountCommandHandler :
        IRequestHandler<UnlinkSubAccountCommand, UnlinkSubAccountCommandResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IAccountRelationshipRepository accountRelationshipRepository;
        private readonly IDomainEventFactory domainEventFactory;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlinkSubAccountCommandHandler"/> class.
        /// </summary>
        /// <param name="accountRepository">Repository for account-related operations.</param>
        /// <param name="accountRelationshipRepository">Repository for account relationship operations.</param>
        /// <param name="domainEventFactory">Factory for creating domain events.</param>
        /// <param name="unitOfWork">Unit of work for persisting changes.</param>
        public UnlinkSubAccountCommandHandler(
            IAccountRepository accountRepository,
            IAccountRelationshipRepository accountRelationshipRepository,
            IDomainEventFactory domainEventFactory,
            IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.accountRelationshipRepository = accountRelationshipRepository;
            this.domainEventFactory = domainEventFactory;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles the <see cref="UnlinkSubAccountCommand"/> request.
        /// </summary>
        /// <param name="request">The command containing unlinking information.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// A <see cref="UnlinkSubAccountCommandResponse"/> containing details of the unlinking operation.
        /// </returns>
        /// <exception cref="AccountAlreadyExistsException">
        /// Thrown when either the head account or sub-account does not exist.
        /// </exception>
        /// <exception cref="AccountRelationshipNotFoundException">
        /// Thrown when no existing relationship is found between the head account and sub-account.
        /// </exception>
        public async Task<UnlinkSubAccountCommandResponse> Handle(
            UnlinkSubAccountCommand request,
            CancellationToken cancellationToken)
        {
            if (!await accountRepository.CheckAccountExistsAsync(request.UnlinkAccountInfo.HeadAccountId))
            {
                throw new AccountAlreadyExistsException(new Domain.Errors.ErrorResponses
                {
                    Code = "HeadAccountNotFound",
                    Message = $"Head account with ID {request.UnlinkAccountInfo.HeadAccountId} does not exist.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            if (!await accountRepository.CheckAccountExistsAsync(request.UnlinkAccountInfo.SubAccountId))
            {
                throw new AccountAlreadyExistsException(new Domain.Errors.ErrorResponses
                {
                    Code = "SubAccountNotFound",
                    Message = $"Sub Account with ID {request.UnlinkAccountInfo.SubAccountId} does not exist.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            if (string.Equals(request.UnlinkAccountInfo.HeadAccountId.ToString(), request.UnlinkAccountInfo.SubAccountId.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                throw new AccountRelationshipNotFoundException(new Domain.Errors.ErrorResponses
                {
                    Code = "InvalidUnlinkOperation",
                    Message = $"Head Account ID and Sub Account ID cannot be the same for unlinking operation.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                    Details = new Domain.Errors.ErrorInfo
                    {
                        AdditionalInfo = new Dictionary<string, string>
                        {
                            { "HeadAccountId", request.UnlinkAccountInfo.HeadAccountId.ToString() },
                            { "SubAccountId", request.UnlinkAccountInfo.SubAccountId.ToString() },
                        },
                    },
                });
            }

            var accRelationship =
                await accountRelationshipRepository.GetAccountRelationshipAsync(
                    request.UnlinkAccountInfo.HeadAccountId,
                    request.UnlinkAccountInfo.SubAccountId);

            if (accRelationship is null)
            {
                throw new AccountRelationshipNotFoundException(new Domain.Errors.ErrorResponses
                {
                    Code = "AccountRelationshipNotFound",
                    Message =
                        $"No existing relationship found between Head Account ID {request.UnlinkAccountInfo.HeadAccountId} " +
                        $"and Sub Account ID {request.UnlinkAccountInfo.SubAccountId}.",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.UtcNow.ToString(),
                    Details = new Domain.Errors.ErrorInfo
                    {
                        AdditionalInfo = new Dictionary<string, string>
                        {
                            { "HeadAccountId", request.UnlinkAccountInfo.HeadAccountId.ToString() },
                            { "SubAccountId", request.UnlinkAccountInfo.SubAccountId.ToString() },
                        },
                    },
                });
            }

            var updateRelationshipInfo = await accountRelationshipRepository.UnlinkSubAccountAsync(request.UnlinkAccountInfo);

            await accountRepository.UnlinkSubAccountAsync(
                request.UnlinkAccountInfo.HeadAccountId,
                request.UnlinkAccountInfo.SubAccountId);

            KafkaProducedEventDto kafkaProducedEventDto = new KafkaProducedEventDto
            {
                AccountId = request.UnlinkAccountInfo.SubAccountId,
                EventType = EventTypes.AccountUnlinked,
                ProducerService = "AccountManager",
                Payload = System.Text.Json.JsonSerializer.Serialize(updateRelationshipInfo),
                ProducedAt = DateTime.UtcNow,
                CorrelationId = Guid.NewGuid(),
            };

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new UnlinkSubAccountCommandResponse
            {
                SubAccountId = request.UnlinkAccountInfo.SubAccountId,
                FormerHeadAccountId = request.UnlinkAccountInfo.HeadAccountId,
                UnlinkedAt = updateRelationshipInfo.UnlinkedAt,
                UnlinkedBy = updateRelationshipInfo.UnlinkedBy,
                Reason = updateRelationshipInfo.Reason,
            };
        }
    }
}
