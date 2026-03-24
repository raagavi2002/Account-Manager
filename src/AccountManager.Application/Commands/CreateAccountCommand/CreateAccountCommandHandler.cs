// <copyright file="CreateAccountCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.CreateAccountCommand
{
    using System.Text.Json;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Interfaces;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Errors;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using MediatR;

    /// <summary>
    /// Handles the creation of accounts by processing <see cref="CreateAccountCommand"/>.
    /// </summary>
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventFactory domainEventFactory;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountCommandHandler"/> class.
        /// </summary>
        /// <param name="domainEventFactory">The domain event factory.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mediator">The mediator.</param>
        public CreateAccountCommandHandler(IDomainEventFactory domainEventFactory, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.domainEventFactory = domainEventFactory;
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
        }

        /// <summary>
        /// Handles the create account command.
        /// </summary>
        /// <param name="request">The create account command request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing account details.</returns>
        public async Task<CreateAccountCommandResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Account.AccountType == null)
                {
                    throw new ArgumentNullException(nameof(request.Account.AccountType));
                }

                if (request.Account.Currency == null)
                {
                    throw new ArgumentNullException(nameof(request.Account.Currency));
                }

                if (string.IsNullOrEmpty(request.Account.AccountName))
                {
                    throw new ArgumentNullException(nameof(request.Account.AccountName));
                }

                if (await unitOfWork.Accounts.CheckAccountExistsAsync(request.Account.AccountName))
                {
                    throw new AccountAlreadyExistsException(new ErrorResponses
                    {
                        Message = "Account name already exists",
                        CorrelationId = Guid.NewGuid().ToString(),
                        Details = new ErrorInfo
                        {
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "AccountName", request.Account.AccountName },
                            },
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (!EnumParser.TryParse<AccountType>(request.Account.AccountType, out var accountType))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Invalid Account Type",
                        Details = new ErrorInfo
                        {
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "AccountType", request.Account.AccountType },
                            },
                        },
                        CorrelationId = Guid.NewGuid().ToString(),
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (!EnumParser.TryParse<CurrencyCodes>(request.Account.Currency, out var currency))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Invalid Currency",
                        Details = new ErrorInfo
                        {
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "Currencycode", request.Account.Currency },
                            },
                        },
                        CorrelationId = Guid.NewGuid().ToString(),
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (request.Account.AccountType == EnumParser.GetEnumMemberValue(AccountType.Professional) && request.Account.InvoiceType != EnumParser.GetEnumMemberValue(BilllingType.OnlinePayment))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Professional accounts are only allowed to choose OnlinePayment",
                        Details = new ErrorInfo
                        {
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "AccountType", request.Account.AccountType },
                            },
                        },
                        CorrelationId = Guid.NewGuid().ToString(),
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }

                if (request.Account.Timezone is null)
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Timezone id cannot be null",
                        Details = new ErrorInfo
                        {
                            AdditionalInfo = new Dictionary<string, string>
                            {
                                { "Timezone", "null" },
                            },
                        },
                        CorrelationId = Guid.NewGuid().ToString(),
                        TimeStamp = DateTime.UtcNow.ToString(),
                    });
                }
                else
                {
                    bool isValidTimezone = await unitOfWork.Accounts.CheckTimezoneExistsAsync(request.Account.Timezone);
                    if (!isValidTimezone)
                    {
                        throw new AccountValidationException(new ErrorResponses
                        {
                            Message = "Invalid Timezone id",
                            Details = new ErrorInfo
                            {
                                AdditionalInfo = new Dictionary<string, string>
                            {
                                { "Timezone", request.Account.Timezone },
                            },
                            },
                            CorrelationId = Guid.NewGuid().ToString(),
                            TimeStamp = DateTime.UtcNow.ToString(),
                        });
                    }
                }

                if (request.Account.AccountManagerId != null)
                {
                    bool isUserExists = await unitOfWork.User.CheckUserIdExistsAsync(request.Account.AccountManagerId ?? Guid.Empty, cancellationToken);
                    if (!isUserExists)
                    {
                        throw new AccountValidationException(new ErrorResponses
                        {
                            Message = "User with given id does not exist",
                            TimeStamp = DateTime.UtcNow.ToString(),
                            Code = "User Not Found",
                            CorrelationId = Guid.NewGuid().ToString(),
                            Details = new ErrorInfo
                            {
                                AdditionalInfo = new Dictionary<string, string>
                                {
                                    { "AccountManagerId", request.Account.AccountManagerId.ToString() ?? string.Empty },
                                },
                            },
                        });
                    }
                }

                if (request.Account.CsmId != null)
                {
                    bool isUserExists = await unitOfWork.User.CheckUserIdExistsAsync(request.Account.CsmId ?? Guid.Empty, cancellationToken);
                    if (!isUserExists)
                    {
                        throw new AccountValidationException(new ErrorResponses
                        {
                            Message = "User with given id does not exist",
                            TimeStamp = DateTime.UtcNow.ToString(),
                            Code = "User Not Found",
                            CorrelationId = Guid.NewGuid().ToString(),
                            Details = new ErrorInfo
                            {
                                AdditionalInfo = new Dictionary<string, string>
                                {
                                    { "CsmId", request.Account.CsmId.ToString() ?? string.Empty },
                                },
                            },
                        });
                    }
                }

                var accountInfo = await unitOfWork.Accounts.CreateAccountAsync(request.Account);
                var accountCreatedEvent = domainEventFactory.CreateAccountCreatedEvent(accountInfo.AccountId, request.Account);
                KafkaProducedEventDto kafkaProducedEvent = new KafkaProducedEventDto
                {
                    AccountId = accountInfo.AccountId,
                    CorrelationId = accountCreatedEvent.CorrelationId,
                    EventType = accountCreatedEvent.EventType,
                    Payload = JsonSerializer.Serialize(accountCreatedEvent),
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
                    EntityId = accountInfo.AccountId,
                    OperationType = Domain.Enums.AuditLog.AuditOperation.AccountCreated,
                    UserId = Guid.NewGuid(),
                    AfterState = JsonSerializer.Serialize(accountInfo),
                    Outcome = Domain.Enums.AuditLog.AuditOutcome.Success,
                    Reason = "Account created",
                }, cancellationToken);

                return new CreateAccountCommandResponse
                {
                    AccountId = accountInfo.AccountId,
                    AccountName = accountInfo.AccountName,
                    AccountType = accountInfo.AccountType,
                    AccountStatus = accountInfo.AccountStatus,
                    Currency = accountInfo.Currency,
                    Timezone = accountInfo.Timezone,
                    Version = accountInfo.Version,
                    CreatedAt = accountInfo.CreatedAt,
                    UpdatedAt = accountInfo.UpdatedAt,
                };
            }
            catch (AccountAlreadyExistsException)
            {
                throw;
            }
            catch (AccountValidationException)
            {
                throw;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error creating account", ex);
            }
        }
    }
}
