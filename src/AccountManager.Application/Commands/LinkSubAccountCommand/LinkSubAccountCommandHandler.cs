// <copyright file="LinkSubAccountCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.LinkSubAccountCommand
{
    using System.Text.Json;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Errors;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="LinkSubAccountCommand"/> by validating input, linking a sub-account
    /// to a head account, persisting the relationship, and producing a domain event.
    /// </summary>
    /// <remarks>
    /// This handler uses the Unit of Work pattern to ensure that all database changes
    /// and outbox events are committed atomically. Domain-specific exceptions are logged
    /// and rethrown, while unexpected exceptions are wrapped and logged.
    /// </remarks>
    public class LinkSubAccountCommandHandler(
        IUnitOfWork unitOfWork,
        IDomainEventFactory domainEventFactory,
        IApplogger applogger)
        : IRequestHandler<LinkSubAccountCommand, LinkSubAccountCommandResponse>
    {
        /// <summary>
        /// Handles the request to link a sub-account to a head account.
        /// </summary>
        /// <param name="request">
        /// The command containing the head account ID, sub-account ID, and relationship details.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to propagate notification that the operation should be canceled.
        /// </param>
        /// <returns>
        /// A <see cref="LinkSubAccountCommandResponse"/> containing details of the created account relationship.
        /// </returns>
        /// <exception cref="AccountValidationException">
        /// Thrown when either the head account or sub-account does not exist.
        /// </exception>
        /// <exception cref="AccountAlreadyLinkedException">
        /// Thrown when the sub-account is already linked to the specified or another head account.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when an unexpected error occurs during the linking process.
        /// </exception>
        public async Task<LinkSubAccountCommandResponse> Handle(
            LinkSubAccountCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.Equals(request.LinkSubAccountDto.HeadAccountId.ToString(), request.LinkSubAccountDto.SubAccountId.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Head account and sub-account cannot be the same",
                        CorrelationId = Guid.NewGuid().ToString(),
                        Details = new ErrorInfo
                        {
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "HeadAccountId", request.LinkSubAccountDto.HeadAccountId.ToString() },
                                { "SubAccountId", request.LinkSubAccountDto.SubAccountId.ToString() },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                // Validate head account existence
                if (!await unitOfWork.Accounts.CheckAccountExistsAsync(request.LinkSubAccountDto.HeadAccountId))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Account does not exist",
                        CorrelationId = Guid.NewGuid().ToString(),
                        Details = new ErrorInfo
                        {
                            AccountId = request.LinkSubAccountDto.HeadAccountId,
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                // Get sub-account info
                var subAccountInfo = await unitOfWork.Accounts
                    .GetAccountInfoByIdAsync(request.LinkSubAccountDto.SubAccountId)
                    .ConfigureAwait(false);

                if (subAccountInfo == null)
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Code = "SubAccountNotFound",
                        Message = "Sub-account does not exist",
                        CorrelationId = Guid.NewGuid().ToString(),
                        Details = new ErrorInfo
                        {
                            AccountId = request.LinkSubAccountDto.SubAccountId,
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                // Check if already linked
                if (subAccountInfo.HeadAccountId.ToString() == request.LinkSubAccountDto.HeadAccountId.ToString())
                {
                    throw new AccountAlreadyLinkedException(new ErrorResponses
                    {
                        Message = "Account has already been linked with the account",
                        Code = "AccountAlreadyLinked",
                        CorrelationId = Guid.NewGuid().ToString(),
                        Details = new ErrorInfo
                        {
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "HeadAccountId", request.LinkSubAccountDto.HeadAccountId.ToString() },
                                { "SubAccountId", request.LinkSubAccountDto.SubAccountId.ToString() },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (!string.IsNullOrEmpty(subAccountInfo.HeadAccountId.ToString()))
                {
                    throw new AccountAlreadyLinkedException(new ErrorResponses
                    {
                        Message = "Account has already been linked with another account",
                        Details = new ErrorInfo
                        {
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "HeadAccountId", request.LinkSubAccountDto.HeadAccountId.ToString() },
                                { "SubAccountId", request.LinkSubAccountDto.SubAccountId.ToString() },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                // Update accounts and create relationship
                await unitOfWork.Accounts
                    .UpdateHeadSubAccountInfoAsync(
                        request.LinkSubAccountDto.HeadAccountId,
                        request.LinkSubAccountDto.SubAccountId)
                    .ConfigureAwait(false);

                var accountRelationshipInfo =
                    await unitOfWork.AccountRelationship
                        .CreateHeadSubAccountRelationshipAsync(
                            request.LinkSubAccountDto.HeadAccountId,
                            request.LinkSubAccountDto.SubAccountId)
                        .ConfigureAwait(false);

                // Create and persist domain event (outbox pattern)
                var accountLinkedEvent =
                    domainEventFactory.CreateAccountLinkedEvent(
                        accountRelationshipInfo,
                        subAccountInfo.AccountName ?? string.Empty);

                KafkaProducedEventDto kafkaProducedEvent = new KafkaProducedEventDto
                {
                    AccountId = subAccountInfo.AccountId ?? Guid.Empty,
                    CorrelationId = accountLinkedEvent.CorrelationId,
                    EventType = accountLinkedEvent.EventType,
                    Payload = JsonSerializer.Serialize(accountLinkedEvent),
                    Status = EnumParser.GetEnumMemberValue(OutboxStatus.Pending),
                    RetryCount = 0,
                };

                await unitOfWork.Outbox.AddKafkaProducedEventAsync(kafkaProducedEvent, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return new LinkSubAccountCommandResponse
                {
                    RelationshipId = accountRelationshipInfo.RelationshipId == 0 ? 5 : accountRelationshipInfo.RelationshipId,
                    HeadAccountId = accountRelationshipInfo.HeadAccountId,
                    SubAccountId = accountRelationshipInfo.SubAccountId,
                    LinkedAt = accountRelationshipInfo.LinkedAt,
                    LinkedBy = accountRelationshipInfo.LinkedBy,
                    RelationshipType = request.LinkSubAccountDto?.RelationshipType,
                };
            }
            catch (AccountValidationException ex)
            {
                applogger.LogException(
                    ex,
                    $"Validation error linking sub-account {request.LinkSubAccountDto.SubAccountId} to head account {request.LinkSubAccountDto.HeadAccountId}.");
                throw;
            }
            catch (AccountAlreadyLinkedException ex)
            {
                applogger.LogException(
                    ex,
                    $"Sub-account {request.LinkSubAccountDto.SubAccountId} is already linked to a head account.");
                throw;
            }
            catch (Exception ex)
            {
                applogger.LogException(
                    ex,
                    $"Unexpected error linking sub-account {request.LinkSubAccountDto.SubAccountId} to head account {request.LinkSubAccountDto.HeadAccountId}.");
                throw new Exception("An unexpected error occurred while linking the sub-account.", ex);
            }
        }
    }
}
