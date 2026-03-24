// <copyright file="UpdateAccountCommandHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Commands.UpdateAccountCommand;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Domain.Results;
    using AccountManager.Shared.Logging;
    using MediatR;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="UpdateAccountCommandHandler"/>.
    /// </summary>
    public class UpdateAccountCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IAccountRepository> accountRepositoryMock;
        private readonly Mock<IOutboxRepository> outboxRepositoryMock;
        private readonly Mock<IDomainEventFactory> domainEventFactoryMock;
        private readonly Mock<IApplogger> apploggerMock;
        private readonly Mock<IMediator> mediatorMock;

        private readonly UpdateAccountCommandHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAccountCommandHandlerTests"/> class.
        /// </summary>
        public UpdateAccountCommandHandlerTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            accountRepositoryMock = new Mock<IAccountRepository>();
            outboxRepositoryMock = new Mock<IOutboxRepository>();
            domainEventFactoryMock = new Mock<IDomainEventFactory>();
            apploggerMock = new Mock<IApplogger>();
            mediatorMock = new Mock<IMediator>();

            unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.Outbox).Returns(outboxRepositoryMock.Object);

            handler = new UpdateAccountCommandHandler(
                unitOfWorkMock.Object,
                apploggerMock.Object,
                domainEventFactoryMock.Object, mediatorMock.Object);
        }

        /// <summary>
        /// Verifies that a valid update request returns a successful response.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_ValidRequest_ReturnsUpdateAccountCommandResponse()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            var updateDto = new UpdateAccountDto
            {
                AccountId = accountId,
                AccountName = "Updated Account",
                AccountType = EnumParser.GetEnumMemberValue(AccountType.Corporate),
                Currency = EnumParser.GetEnumMemberValue(CurrencyCodes.EUR),
                Timezone = EnumParser.GetEnumMemberValue(Domain.Enums.TimeZone.UTC),
                BillingType = EnumParser.GetEnumMemberValue(BilllingType.OnlinePayment),
                Version = 1,
            };

            var command = new UpdateAccountCommand
            {
                UpdateAccountDto = updateDto,
            };

            var updatedAccount = new UpdateAccountResult
            {
                AccountId = accountId,
                Version = 2,
            };

            var updatedMetadata = new List<FieldChangeDto>
            {
                new FieldChangeDto
                {
                    Field = "AccountName",
                    OldValue = "acc_name",
                    NewValue = "acc_new_name",
                },
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(updateDto.AccountName))
                .ReturnsAsync(false);

            accountRepositoryMock
                .Setup(r => r.UpdateAccountAsync(updateDto))
                .ReturnsAsync((updatedAccount, updatedMetadata));

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountId, result.AccountId);
            Assert.Equal(updatedAccount.Version, result.Version);
        }

        /// <summary>
        /// Verifies that an <see cref="ArgumentException"/> is thrown when the account name is missing.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_MissingAccountName_ThrowsArgumentException()
        {
            // Arrange
            var command = new UpdateAccountCommand
            {
                UpdateAccountDto = new UpdateAccountDto
                {
                    AccountId = Guid.NewGuid(),
                    Version = 1,
                },
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that an <see cref="AccountAlreadyExistsException"/> is thrown
        /// when the account name already exists.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_AccountAlreadyExists_ThrowsAccountAlreadyExistsException()
        {
            // Arrange
            var updateDto = new UpdateAccountDto
            {
                AccountId = Guid.NewGuid(),
                AccountName = "DuplicateName",
                AccountType = "Standard",
                Currency = "EUR",
                Timezone = "UTC",
                Version = 1,
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(updateDto.AccountName))
                .ReturnsAsync(true);

            var command = new UpdateAccountCommand { UpdateAccountDto = updateDto };

            // Act & Assert
            await Assert.ThrowsAsync<AccountAlreadyExistsException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that an invalid account type results in an <see cref="AccountValidationException"/>.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_InvalidAccountType_ThrowsAccountValidationException()
        {
            // Arrange
            var updateDto = new UpdateAccountDto
            {
                AccountId = Guid.NewGuid(),
                AccountName = "Test",
                AccountType = "InvalidType",
                Currency = "EUR",
                Timezone = "UTC",
                Version = 1,
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(updateDto.AccountName))
                .ReturnsAsync(false);

            var command = new UpdateAccountCommand { UpdateAccountDto = updateDto };

            // Act & Assert
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that a Professional account with invalid billing type throws an exception.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_ProfessionalAccountWithInvalidBillingType_ThrowsAccountValidationException()
        {
            // Arrange
            var updateDto = new UpdateAccountDto
            {
                AccountId = Guid.NewGuid(),
                AccountName = "ProAccount",
                AccountType = EnumParser.GetEnumMemberValue(AccountType.Professional),
                Currency = "EUR",
                Timezone = "UTC",
                BillingType = "Offline",
                Version = 1,
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(updateDto.AccountName))
                .ReturnsAsync(false);

            var command = new UpdateAccountCommand { UpdateAccountDto = updateDto };

            // Act & Assert
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that an invalid timezone results in an <see cref="AccountValidationException"/>.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_InvalidTimezone_ThrowsAccountValidationException()
        {
            // Arrange
            var updateDto = new UpdateAccountDto
            {
                AccountId = Guid.NewGuid(),
                AccountName = "Test",
                AccountType = "Standard",
                Currency = "EUR",
                Timezone = "InvalidTZ",
                Version = 1,
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(updateDto.AccountName))
                .ReturnsAsync(false);

            var command = new UpdateAccountCommand { UpdateAccountDto = updateDto };

            // Act & Assert
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
