// <copyright file="CreateAccountCommandHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Commands.CreateAccountCommand;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Domain.Results;
    using MediatR;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="CreateAccountCommandHandler"/>.
    /// </summary>
    public class CreateAccountCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IAccountRepository> accountRepositoryMock;
        private readonly Mock<IDomainEventFactory> domainEventFactoryMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly CreateAccountCommandHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountCommandHandlerTests"/> class.
        /// </summary>
        public CreateAccountCommandHandlerTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            accountRepositoryMock = new Mock<IAccountRepository>();
            unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepositoryMock.Object);
            domainEventFactoryMock = new Mock<IDomainEventFactory>();
            mediatorMock = new Mock<IMediator>();

            handler = new CreateAccountCommandHandler(
                domainEventFactoryMock.Object,
                unitOfWorkMock.Object,
                mediatorMock.Object);
        }

        /// <summary>
        /// Verifies that a valid account creation request returns a successful response.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_ValidRequest_ReturnsCreateAccountCommandResponse()
        {
            // Arrange
            var accountDto = new CreateAccountDto
            {
                AccountName = "Test Account",
                AccountType = EnumParser.GetEnumMemberValue(AccountType.Enterprise),
                Currency = EnumParser.GetEnumMemberValue(CurrencyCodes.EUR),
                Timezone = "Asia/Kolkata",
                InvoiceType = EnumParser.GetEnumMemberValue(BilllingType.OnlinePayment),
                Address = new AddressDto
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "State",
                    PostalCode = "12345",
                    Country = "Country",
                },
            };

            var command = new CreateAccountCommand
            {
                Account = accountDto,
            };

            var createdAccount = new AccountDto
            {
                AccountId = Guid.NewGuid(),
                AccountName = accountDto.AccountName,
                AccountType = accountDto.AccountType,
                Currency = accountDto.Currency,
                Timezone = "USD",
                AccountStatus = "Active",
                Version = 1,
            };

            var expectedResponse = new CreateAccountResult
            {
                AccountId = createdAccount.AccountId.Value,
                AccountName = createdAccount.AccountName!,
                AccountType = createdAccount.AccountType!,
                Currency = createdAccount.Currency!,
                Timezone = createdAccount.Timezone!,
                AccountStatus = createdAccount.AccountStatus!,
                Version = createdAccount.Version,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            unitOfWorkMock
                .Setup(u => u.Accounts.CheckAccountExistsAsync(accountDto.AccountName))
                .ReturnsAsync(false);

            unitOfWorkMock.Setup(u => u.Accounts.CreateAccountAsync(accountDto)).ReturnsAsync(expectedResponse);

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdAccount.AccountId, result.AccountId);
            Assert.Equal(createdAccount.AccountName, result.AccountName);
            Assert.Equal(createdAccount.AccountType, result.AccountType);
            Assert.Equal(createdAccount.Currency, result.Currency);
        }

        /// <summary>
        /// Verifies that an <see cref="ArgumentNullException"/> is thrown when account name is missing.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_MissingAccountName_ThrowsArgumentNullException()
        {
            // Arrange
            var command = new CreateAccountCommand
            {
                Account = new CreateAccountDto
                {
                    AccountName = null!,
                    Address = new AddressDto()
                    {
                        City = "City",
                        Country = "Country",
                        PostalCode = "12345",
                        State = "State",
                        Street = "Street",
                    },
                    AccountType = "Standard",
                    Currency = "EUR",
                    Timezone = "Asia/Kolkata",
                },
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
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
            var command = new CreateAccountCommand
            {
                Account = new CreateAccountDto
                {
                    AccountName = "ExistingAccount",
                    AccountType = "Standard",
                    Currency = "EUR",
                    Timezone = "Asia/Kolkata",
                    Address = new AddressDto
                    {
                        Street = "123 Main St",
                        City = "Anytown",
                        State = "State",
                        PostalCode = "12345",
                        Country = "Country",
                    },
                },
            };

            unitOfWorkMock
                .Setup(u => u.Accounts.CheckAccountExistsAsync(command.Account.AccountName))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<AccountAlreadyExistsException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that an <see cref="AccountValidationException"/> is thrown
        /// for an invalid account type.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_InvalidAccountType_ThrowsAccountValidationException()
        {
            // Arrange
            var command = new CreateAccountCommand
            {
                Account = new CreateAccountDto
                {
                    AccountName = "TestAccount",
                    AccountType = "InvalidType",
                    Currency = "EUR",
                    Timezone = "Asia/Kolkata",
                    Address = new AddressDto
                    {
                        Street = "123 Main St",
                        City = "Anytown",
                        State = "State",
                        PostalCode = "12345",
                        Country = "Country",
                    },
                },
            };

            unitOfWorkMock
                .Setup(u => u.Accounts.CheckAccountExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that Professional accounts cannot use non-online payment invoice types.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_ProfessionalAccountWithInvalidInvoiceType_ThrowsAccountValidationException()
        {
            // Arrange
            var command = new CreateAccountCommand
            {
                Account = new CreateAccountDto
                {
                    AccountName = "ProAccount",
                    AccountType = EnumParser.GetEnumMemberValue(AccountType.Professional),
                    Currency = "EUR",
                    Timezone = "Asia/Kolkata",
                    InvoiceType = "Offline",
                    Address = new AddressDto
                    {
                        Street = "123 Main St",
                        City = "Anytown",
                        State = "State",
                        PostalCode = "12345",
                        Country = "Country",
                    },
                },
            };

            unitOfWorkMock
                .Setup(u => u.Accounts.CheckAccountExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

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
            var command = new CreateAccountCommand
            {
                Account = new CreateAccountDto
                {
                    AccountName = "TestAccount",
                    AccountType = "Standard",
                    Currency = "EUR",
                    Timezone = "Asia",
                    Address = new AddressDto
                        {
                            Street = "123 Main St",
                            City = "Anytown",
                            State = "State",
                            PostalCode = "12345",
                            Country = "Country",
                        },
                },
            };

            unitOfWorkMock
                .Setup(u => u.Accounts.CheckAccountExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
