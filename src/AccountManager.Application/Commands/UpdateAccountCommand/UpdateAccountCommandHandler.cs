// <copyright file="UpdateAccountCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.UpdateAccountCommand
{
    using System.Text.Json;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Errors;
    using AccountManager.Domain.Events.Published;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using MediatR;

    /// <summary>
    /// Handles the <see cref="UpdateAccountCommand"/> by executing the account update operation.
    /// </summary>
    /// <remarks>
    /// This handler is responsible for validating the update request,
    /// applying business rules, and persisting changes to the underlying data store.
    /// </remarks>
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, UpdateAccountCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IApplogger applogger;
        private readonly IDomainEventFactory domainEventFactory;
        private readonly IMediator mediator;

        public UpdateAccountCommandHandler(IUnitOfWork unitOfWork, IApplogger applogger, IDomainEventFactory domainEventFactory, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.applogger = applogger;
            this.domainEventFactory = domainEventFactory;
            this.mediator = mediator;
        }

        /// <summary>
        /// Handles the update account command by validating the request, applying business rules, updating the account,
        /// and persisting the changes to the data store.
        /// </summary>
        /// <param name="request">The update account command containing the account update information.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation, with a result of <see cref="UpdateAccountCommandResponse"/>
        /// containing the updated account information.
        /// </returns>
        /// <exception cref="AccountValidationException">Thrown when the account data is invalid.</exception>
        /// <exception cref="AccountAlreadyExistsException">Thrown when the account name already exists.</exception>
        /// <exception cref="ArgumentException">Thrown when required arguments are missing or invalid.</exception>
        public async Task<UpdateAccountCommandResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: Implement the permission and version check here.
                var accountName = request.UpdateAccountDto.AccountName;
                if (string.IsNullOrWhiteSpace(accountName))
                {
                    throw new ArgumentException(
                        "Account name must not be null or empty.",
                        nameof(request.UpdateAccountDto.AccountName));
                }

                if (await unitOfWork.Accounts.CheckAccountExistsAsync(accountName))
                {
                    throw new AccountAlreadyExistsException(new ErrorResponses
                    {
                        Message = "Account name already exists",
                        Details = new ErrorInfo
                        {
                            AccountId = request.UpdateAccountDto.AccountId,
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "AccountName", accountName },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (!EnumParser.TryParse<AccountType>(request.UpdateAccountDto.AccountType ?? string.Empty, out var accountType))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Invalid Account Type",
                        Details = new ErrorInfo
                        {
                            AccountId = request.UpdateAccountDto.AccountId,
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "AccountType", request.UpdateAccountDto.AccountType ?? string.Empty },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (!EnumParser.TryParse<CurrencyCodes>(request.UpdateAccountDto.Currency ?? string.Empty, out var currency))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Invalid Currency",
                        Details = new ErrorInfo
                        {
                            AccountId = request.UpdateAccountDto.AccountId,
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "Currencycode", request.UpdateAccountDto.Currency ?? string.Empty },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (request.UpdateAccountDto.AccountType == EnumParser.GetEnumMemberValue(AccountType.Professional) &&
                    request?.UpdateAccountDto?.BillingType?.ToUpper() != EnumParser.GetEnumMemberValue<BilllingType>(BilllingType.OnlinePayment))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Professional accounts are only allowed to choose OnlinePayment",
                        Details = new ErrorInfo
                        {
                            AccountId = request.UpdateAccountDto.AccountId,
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "AccountType", request.UpdateAccountDto.AccountType ?? string.Empty },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (!EnumParser.TryParse<Domain.Enums.TimeZone>(request.UpdateAccountDto.Timezone ?? string.Empty, out var timezone))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Invalid Timezone",
                        Details = new ErrorInfo
                        {
                            AccountId = request.UpdateAccountDto.AccountId,
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "Timezone", request.UpdateAccountDto.Timezone ?? string.Empty },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                var updatedInfo = await unitOfWork.Accounts.UpdateAccountAsync(request.UpdateAccountDto).ConfigureAwait(false);

                var updateEvent = domainEventFactory.CreateAccountUpdatedEvent(request.UpdateAccountDto, updatedInfo.Item2);

                KafkaProducedEventDto kafkaProducedEvent = new KafkaProducedEventDto
                {
                    AccountId = request.UpdateAccountDto.AccountId ?? Guid.Empty,
                    CorrelationId = updateEvent.CorrelationId,
                    EventType = updateEvent.EventType,
                    Payload = JsonSerializer.Serialize(updateEvent),
                    Status = EnumParser.GetEnumMemberValue(OutboxStatus.Pending),
                    RetryCount = 0,
                };

                await unitOfWork.Outbox.AddKafkaProducedEventAsync(kafkaProducedEvent, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                // Publish audit log
                await mediator.Send(
                    new Commands.CreateAuditEntryCommand.CreateAuditEntryCommand
                {
                    EntityType = Domain.Enums.AuditLog.AuditEntityType.Account,
                    EntityId = request.UpdateAccountDto.AccountId ?? Guid.Empty,
                    OperationType = Domain.Enums.AuditLog.AuditOperation.AccountUpdated,
                    UserId = Guid.NewGuid(),
                    BeforeState = JsonSerializer.Serialize(updatedInfo.Item1),
                    AfterState = JsonSerializer.Serialize(updatedInfo.Item1),
                    ChangedFields = JsonSerializer.Serialize(updatedInfo.Item2),
                    Outcome = Domain.Enums.AuditLog.AuditOutcome.Success,
                    Reason = "Account updated",
                }, cancellationToken);

                return new UpdateAccountCommandResponse
                {
                    AccountId = request.UpdateAccountDto.AccountId ?? Guid.Empty,
                    Version = updatedInfo.Item1.Version,
                    UpdatedAt = updatedInfo.Item1.UpdatedAt,
                    UpdatedBy = updatedInfo.Item1.UpdatedBy,
                };
            }
            catch (AccountValidationException)
            {
                throw;
            }
            catch (AccountAlreadyExistsException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                applogger.LogException(
                    ex,
                    $"Unhandled exception while updating account. AccountId: {request?.UpdateAccountDto?.AccountId}");

                throw;
            }
        }
    }
}
