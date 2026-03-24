// <copyright file="AccountStatusTransitCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.AccountStatusTransitCommand
{
    using System.Text.Json;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Errors;
    using AccountManager.Domain.Events.Published;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Helpers;
    using AccountManager.Domain.Interfaces;
    using MediatR;
    using TimeZone = AccountManager.Domain.Enums.TimeZone;

    /// <summary>
    /// Handles the <see cref="AccountStatusTransitCommand"/> by validating business rules
    /// and transitioning an account to the requested status.
    /// </summary>
    /// <remarks>
    /// This handler enforces domain constraints such as required account data,
    /// valid status transitions, and user-role requirements before persisting changes.
    /// </remarks>
    public class AccountStatusTransitCommandHandler(IAccountRepository accountRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IDomainEventFactory domainEventFactory) : IRequestHandler<AccountStatusTransitCommand, AccountStatusTransitCommandResponse>
    {
        /// <summary>
        /// Handles the account status transition request.
        /// </summary>
        /// <param name="request">
        /// The command containing the account identifier, target status,
        /// and the reason for the status transition.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the operation if requested.
        /// </param>
        /// <returns>
        /// An <see cref="AccountStatusTransitCommandResponse"/> describing the
        /// completed status transition.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the account does not exist or required account data is missing.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when one or more business validation rules are violated, such as
        /// attempting to update to the same status or missing required information.
        /// </exception>
        public async Task<AccountStatusTransitCommandResponse> Handle(AccountStatusTransitCommand request, CancellationToken cancellationToken)
        {
            var accountInfo = await accountRepository.GetAccountInfoByIdAsync(request.AccountStatusTransitInfo.AccountId).ConfigureAwait(false);

            if (accountInfo == null)
            {
                throw new AccountValidationException(new ErrorResponses
                {
                    Message = "Account does not exists",
                    Details = new ErrorInfo
                    {
                        AccountId = request.AccountStatusTransitInfo.AccountId,
                    },
                    TimeStamp = DateTime.UtcNow.ToString(),
                    CorrelationId = Guid.NewGuid().ToString(),
                    Code = "ACCOUNT DOES NOT EXIST",
                });
            }

            var statusToBeUpdated = EnumParser.ParseFromEnumMember<AccountStatus>(request.AccountStatusTransitInfo.AccountStatus.ToUpper());

            var existingStatus = EnumParser.ParseFromEnumMember<AccountStatus>(accountInfo.AccountStatus ?? string.Empty);

            if (statusToBeUpdated != existingStatus)
            {
                if (string.IsNullOrEmpty(request.AccountStatusTransitInfo.Reason))
                {
                    throw new AccountValidationException(new ErrorResponses
                    {
                        Message = "Reason Cannot Be Empty",
                        Details = new ErrorInfo
                        {
                            AccountId = request.AccountStatusTransitInfo.AccountId,
                        },
                        TimeStamp = DateTime.UtcNow.ToString(),
                        Code = "REASONISEMPTY",
                        CorrelationId = Guid.NewGuid().ToString(),
                    });
                }

                switch (statusToBeUpdated)
                {
                    case AccountStatus.Inactive:
                        await accountRepository.UpdateAccountStatusAsync(request.AccountStatusTransitInfo.AccountId, EnumParser.GetEnumMemberValue(AccountStatus.Inactive));
                        break;

                    case AccountStatus.Active:
                        if (!await accountRepository.CheckTimezoneExistsAsync(accountInfo.Timezone ?? string.Empty))
                        {
                            throw new AccountValidationException(new ErrorResponses
                            {
                                Message = "Invalid Timezone",
                                Details = new ErrorInfo
                                {
                                    AccountId = request.AccountStatusTransitInfo.AccountId,
                                },
                                TimeStamp = DateTime.UtcNow.ToString(),
                            });
                        }

                        if (accountInfo.Address == null)
                        {
                            throw new AccountValidationException(new ErrorResponses
                            {
                                Message = "Address is required to activate the account",
                                Details = new ErrorInfo
                                {
                                    AccountId = request.AccountStatusTransitInfo.AccountId,
                                },
                                TimeStamp = DateTime.UtcNow.ToString(),
                            });
                        }

                        AccountHelper.IsValidAddress(accountInfo.Address);

                        if (accountInfo.AccountName == null)
                        {
                            throw new ArgumentNullException(nameof(accountInfo.AccountName));
                        }

                        if (!await accountRepository.CheckAccountExistsAsync(accountInfo.AccountName))
                        {
                            throw new AccountAlreadyExistsException(new ErrorResponses
                            {
                                Message = "Account name already exists",
                                Details = new ErrorInfo
                                {
                                    AccountId = request.AccountStatusTransitInfo.AccountId,
                                    AdditionalInfo = new Dictionary<string, string>
                                    {
                                        {"AccountName", accountInfo.AccountName},
                                    },
                                },
                                TimeStamp = DateTime.UtcNow.ToString(),
                            });
                        }

                        if (!await userRepository.HasMainClientUserAsync(request.AccountStatusTransitInfo.AccountId))
                        {
                            throw new AccountValidationException(new ErrorResponses
                            {
                                Message = "Atleast one main client user must be assigned",
                                Details = new ErrorInfo
                                {
                                    AccountId = request.AccountStatusTransitInfo.AccountId,
                                },
                                TimeStamp = DateTime.UtcNow.ToString(),
                                CorrelationId = Guid.NewGuid().ToString(),
                                Code = "MainClientUserRequired",
                            });
                        }

                        await accountRepository.UpdateAccountStatusAsync(request.AccountStatusTransitInfo.AccountId, EnumParser.GetEnumMemberValue<AccountStatus>(AccountStatus.Active));
                        break;
                }

                var accountStatusChangedEvent = domainEventFactory.CreateAccountStatusChangedEvent(request.AccountStatusTransitInfo, accountInfo);
                KafkaProducedEventDto kafkaProducedEvent = new KafkaProducedEventDto
                {
                    AccountId = accountInfo.AccountId ?? Guid.Empty,
                    CorrelationId = accountStatusChangedEvent.CorrelationId,
                    EventType = accountStatusChangedEvent.EventType,
                    Payload = JsonSerializer.Serialize(accountStatusChangedEvent),
                    Status = EnumParser.GetEnumMemberValue(OutboxStatus.Pending),
                    RetryCount = 0,
                };
                await unitOfWork.Outbox.AddKafkaProducedEventAsync(kafkaProducedEvent, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return new AccountStatusTransitCommandResponse
                {
                    AccountId = request.AccountStatusTransitInfo.AccountId,
                    StatusChangedAt = DateTime.UtcNow,
                    AccountStatus = request.AccountStatusTransitInfo.AccountStatus,
                    Reason = request.AccountStatusTransitInfo.Reason,
                    Version = accountInfo.Version++,
                };
            }

            throw new InvalidAccountStatusTransitionException(new ErrorResponses
            {
                Message = "Same status cannot be updated",
                TimeStamp = DateTime.UtcNow.ToString(),
                Details = new ErrorInfo
                {
                    AccountId = request?.AccountStatusTransitInfo.AccountId,
                    AdditionalInfo = new Dictionary<string, string>
                    {
                        { "AccountStatus", request.AccountStatusTransitInfo.AccountStatus },
                    },
                },
                Code = "INVALIDSTATUS",
                CorrelationId = Guid.NewGuid().ToString(),
            });
        }
    }
}
