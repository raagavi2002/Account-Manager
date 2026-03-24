// <copyright file="AccountStatusTransitCommandHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Commands.AccountStatusTransitCommand;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Application.Abstractions;
    using AccountManager.Domain.Events.Published;
    using MediatR;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="AccountStatusTransitCommandHandler"/>.
    /// </summary>
    public class AccountStatusTransitCommandHandlerTests
        {
            private readonly Mock<IAccountRepository> accountRepositoryMock;
            private readonly Mock<IUserRepository> userRepositoryMock;
            private readonly Mock<IUnitOfWork> unitOfWorkMock;
            private readonly Mock<IDomainEventFactory> domainEventFactoryMock;

            private readonly AccountStatusTransitCommandHandler handler;

            /// <summary>
            /// Initializes a new instance of the <see cref="AccountStatusTransitCommandHandlerTests"/> class.
            /// </summary>
            public AccountStatusTransitCommandHandlerTests()
            {
                accountRepositoryMock = new Mock<IAccountRepository>();
                userRepositoryMock = new Mock<IUserRepository>();
                unitOfWorkMock = new Mock<IUnitOfWork>();
                domainEventFactoryMock = new Mock<IDomainEventFactory>();

                handler = new AccountStatusTransitCommandHandler(
                    accountRepositoryMock.Object,
                    userRepositoryMock.Object,
                    unitOfWorkMock.Object,
                    domainEventFactoryMock.Object);
            }

            /// <summary>
            /// Verifies that the handler updates the account status to Inactive
            /// and persists an outbox event when valid input is provided.
            /// </summary>
            /// <returns>
            /// A task that represents the asynchronous test execution.
            /// </returns>
            [Fact]
            public async Task Handle_Should_Update_Status_To_Inactive_When_Request_Is_Valid()
            {
                // Arrange
                Guid accountId = Guid.NewGuid();

                var accountInfo = new AccountDto
                {
                    AccountId = accountId,
                    AccountStatus = "ACTIVE",
                    AccountName = "TestAccount",
                };

                var command = new AccountStatusTransitCommand
                {
                    AccountStatusTransitInfo = new AccountStatusTransitDto
                    {
                        AccountId = accountId,
                        AccountStatus = "INACTIVE",
                        Reason = "Business decision",
                        Version = 1,
                    },
                };

                accountRepositoryMock
                    .Setup(r => r.GetAccountInfoByIdAsync(accountId))
                    .ReturnsAsync(accountInfo);

                accountRepositoryMock
                    .Setup(r => r.UpdateAccountStatusAsync(accountId, It.IsAny<string>(), false))
                    .Returns(Task.CompletedTask);

                domainEventFactoryMock
                    .Setup(f => f.CreateAccountStatusChangedEvent(
                        It.IsAny<AccountStatusTransitDto>(),
                        It.IsAny<AccountDto>()))
                    .Returns(new AccountStatusChangedEvent
                    {
                        CorrelationId = Guid.NewGuid(),
                        EventType = EventTypes.AccountStatusChanged,
                    });

                unitOfWorkMock
                    .Setup(u => u.Outbox.AddKafkaProducedEventAsync(
                        It.IsAny<KafkaProducedEventDto>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                unitOfWorkMock
                    .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                // Act
                var response = await handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.NotNull(response);
                Assert.Equal(accountId, response.AccountId);
                Assert.Equal("INACTIVE", response.AccountStatus);
                Assert.Equal("Business decision", response.Reason);

                accountRepositoryMock.Verify(
                    r => r.UpdateAccountStatusAsync(accountId, It.IsAny<string>(), false),
                    Times.Once);

                unitOfWorkMock.Verify(
                    u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
