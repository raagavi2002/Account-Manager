// <copyright file="LinkSubAccountCommandHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Commands.LinkSubAccountCommand;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Events.Constants;
    using AccountManager.Domain.Events.Published;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Domain.Results;
    using AccountManager.Shared.Logging;
    using Moq;
    using Xunit;

    /// <summary>
    /// Contains unit tests for the <see cref="LinkSubAccountCommandHandler"/> class.
    /// </summary>
    public class LinkSubAccountCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IDomainEventFactory> mockDomainEventFactory;
        private readonly Mock<IApplogger> mockAppLogger;
        private readonly Mock<IAccountRepository> mockAccountRepository;
        private readonly Mock<IAccountRelationshipRepository> mockAccountRelationshipRepository;
        private readonly Mock<IOutboxRepository> mockOutboxRepository;
        private readonly LinkSubAccountCommandHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkSubAccountCommandHandlerTests"/> class.
        /// Sets up common mocks and dependencies used across all tests.
        /// </summary>
        public LinkSubAccountCommandHandlerTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockDomainEventFactory = new Mock<IDomainEventFactory>();
            mockAppLogger = new Mock<IApplogger>();
            mockAccountRepository = new Mock<IAccountRepository>();
            mockAccountRelationshipRepository = new Mock<IAccountRelationshipRepository>();
            mockOutboxRepository = new Mock<IOutboxRepository>();

            mockUnitOfWork.Setup(x => x.Accounts).Returns(mockAccountRepository.Object);
            mockUnitOfWork.Setup(x => x.AccountRelationship).Returns(mockAccountRelationshipRepository.Object);
            mockUnitOfWork.Setup(x => x.Outbox).Returns(mockOutboxRepository.Object);

            handler = new LinkSubAccountCommandHandler(
                mockUnitOfWork.Object,
                mockDomainEventFactory.Object,
                mockAppLogger.Object);
        }

        /// <summary>
        /// Tests that the handler successfully links a sub-account to a head account
        /// when all validations pass and no existing relationship exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_ValidRequest_SuccessfullyLinksSubAccountToHeadAccount()
        {
            // Arrange
            Guid headAccountId = Guid.NewGuid();
            var subAccountId = Guid.NewGuid();
            var relationshipId = 1;

            var command = new LinkSubAccountCommand
            {
                LinkSubAccountDto = new LinkSubAccountDto
                {
                    HeadAccountId = headAccountId,
                    SubAccountId = subAccountId,
                    RelationshipType = "HEAD-SUB",
                },
            };

            var accountInfo = new AccountDto
            {
                AccountId = subAccountId,
                AccountName = "Sub Account",
                HeadAccountId = Guid.Empty,
            };

            var accountRelationshipInfo = new LinkSubAccountResult
            {
                RelationshipId = relationshipId,
                HeadAccountId = headAccountId,
                SubAccountId = subAccountId,
                LinkedAt = DateTime.UtcNow,
                LinkedBy = "System",
            };

            var accountLinkedEvent = new AccountLinkedEvent
            {
                CorrelationId = Guid.NewGuid(),
                EventType = EventTypes.AccountLinked,
            };

            mockAccountRepository.Setup(x => x.CheckAccountExistsAsync(accountInfo.AccountName))
                .ReturnsAsync(true);
            mockAccountRepository.Setup(x => x.GetAccountInfoByIdAsync(subAccountId))
                .ReturnsAsync(accountInfo);
            mockAccountRepository.Setup(x => x.UpdateHeadSubAccountInfoAsync(headAccountId, subAccountId))
                .Returns(Task.CompletedTask);
            mockAccountRelationshipRepository.Setup(x => x.CreateHeadSubAccountRelationshipAsync(headAccountId, subAccountId))
                .ReturnsAsync(accountRelationshipInfo);
            mockDomainEventFactory.Setup(x => x.CreateAccountLinkedEvent(accountRelationshipInfo, accountInfo.AccountName))
                .Returns(accountLinkedEvent);
            mockOutboxRepository.Setup(x => x.AddKafkaProducedEventAsync(It.IsAny<KafkaProducedEventDto>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(relationshipId, result.RelationshipId);
            Assert.Equal(headAccountId, result.HeadAccountId);
            Assert.Equal(subAccountId, result.SubAccountId);
            Assert.Equal("Primary", result.RelationshipType);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Tests that the handler throws an <see cref="AccountValidationException"/>
        /// when the head account does not exist in the system.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_HeadAccountDoesNotExist_ThrowsAccountValidationException()
        {
            // Arrange
            var command = new LinkSubAccountCommand
            {
                LinkSubAccountDto = new LinkSubAccountDto
                {
                    HeadAccountId = Guid.NewGuid(),
                    SubAccountId = Guid.NewGuid(),
                },
            };

            mockAccountRepository.Setup(x => x.CheckAccountExistsAsync(command.LinkSubAccountDto.HeadAccountId))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AccountValidationException>(
                () => handler.Handle(command, CancellationToken.None));

            Assert.Equal("Account does not exist", exception?.Error?.Message);
            Assert.Equal(command.LinkSubAccountDto.HeadAccountId, exception?.Error?.Details?.AccountId);
        }

        /// <summary>
        /// Tests that the handler throws an <see cref="AccountValidationException"/>
        /// when the sub-account does not exist in the system.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_SubAccountDoesNotExist_ThrowsAccountValidationException()
        {
            // Arrange
            var command = new LinkSubAccountCommand
            {
                LinkSubAccountDto = new LinkSubAccountDto
                {
                    HeadAccountId = Guid.NewGuid(),
                    SubAccountId = Guid.NewGuid(),
                },
            };

            mockAccountRepository.Setup(x => x.CheckAccountExistsAsync(command.LinkSubAccountDto.HeadAccountId))
                .ReturnsAsync(true);
            mockAccountRepository.Setup(x => x.GetAccountInfoByIdAsync(command.LinkSubAccountDto.SubAccountId))
                .ReturnsAsync((AccountDto)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AccountValidationException>(
                () => handler.Handle(command, CancellationToken.None));

            Assert.Equal("Account does not exist", exception?.Error?.Message);
            Assert.Equal(command.LinkSubAccountDto.HeadAccountId, exception?.Error?.Details?.AccountId);
        }

        /// <summary>
        /// Tests that the handler throws an <see cref="AccountAlreadyLinkedException"/>
        /// when the sub-account is already linked to the specified head account.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_SubAccountAlreadyLinkedToSameHeadAccount_ThrowsAccountAlreadyLinkedException()
        {
            // Arrange
            var headAccountId = Guid.NewGuid();
            var subAccountId = Guid.NewGuid();

            var command = new LinkSubAccountCommand
            {
                LinkSubAccountDto = new LinkSubAccountDto
                {
                    HeadAccountId = headAccountId,
                    SubAccountId = subAccountId,
                },
            };

            var accountInfo = new AccountDto
            {
                AccountId = subAccountId,
                AccountName = "Sub Account",
                HeadAccountId = headAccountId,
            };

            mockAccountRepository.Setup(x => x.CheckAccountExistsAsync(headAccountId))
                .ReturnsAsync(true);
            mockAccountRepository.Setup(x => x.GetAccountInfoByIdAsync(subAccountId))
                .ReturnsAsync(accountInfo);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AccountAlreadyLinkedException>(
                () => handler.Handle(command, CancellationToken.None));

            Assert.Equal("Account has already been linked with the account", exception?.Error?.Message);
        }

        /// <summary>
        /// Tests that the handler throws an <see cref="AccountAlreadyLinkedException"/>
        /// when the sub-account is already linked to a different head account.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_SubAccountAlreadyLinkedToDifferentHeadAccount_ThrowsAccountAlreadyLinkedException()
        {
            // Arrange
            var headAccountId = Guid.NewGuid();
            var subAccountId = Guid.NewGuid();
            var existingHeadAccountId = Guid.NewGuid();

            var command = new LinkSubAccountCommand
            {
                LinkSubAccountDto = new LinkSubAccountDto
                {
                    HeadAccountId = headAccountId,
                    SubAccountId = subAccountId,
                },
            };

            var accountInfo = new AccountDto
            {
                AccountId = subAccountId,
                AccountName = "Sub Account",
                HeadAccountId = existingHeadAccountId,
            };

            mockAccountRepository.Setup(x => x.CheckAccountExistsAsync(headAccountId))
                .ReturnsAsync(true);
            mockAccountRepository.Setup(x => x.GetAccountInfoByIdAsync(subAccountId))
                .ReturnsAsync(accountInfo);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AccountAlreadyLinkedException>(
                () => handler.Handle(command, CancellationToken.None));

            Assert.Equal("Account has already been linked with another account", exception?.Error?.Message);
        }

        /// <summary>
        /// Tests that the handler creates and persists a Kafka outbox event
        /// with the correct properties when linking accounts successfully.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_ValidRequest_CreatesKafkaOutboxEventWithCorrectProperties()
        {
            // Arrange
            var headAccountId = Guid.NewGuid();
            var subAccountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var command = new LinkSubAccountCommand
            {
                LinkSubAccountDto = new LinkSubAccountDto
                {
                    HeadAccountId = headAccountId,
                    SubAccountId = subAccountId,
                },
            };

            var accountInfo = new AccountDto
            {
                AccountId = subAccountId,
                AccountName = "Sub Account",
                HeadAccountId = Guid.Empty,
            };

            var accountRelationshipInfo = new LinkSubAccountResult
            {
                RelationshipId = 1,
                HeadAccountId = headAccountId,
                SubAccountId = subAccountId,
                LinkedAt = DateTime.UtcNow,
                LinkedBy = "System",
            };

            var accountLinkedEvent = new AccountLinkedEvent
            {
                CorrelationId = Guid.Parse(correlationId),
                EventType = EventTypes.AccountLinked,
            };

            mockAccountRepository.Setup(x => x.CheckAccountExistsAsync(headAccountId))
                .ReturnsAsync(true);
            mockAccountRepository.Setup(x => x.GetAccountInfoByIdAsync(subAccountId))
                .ReturnsAsync(accountInfo);
            mockAccountRepository.Setup(x => x.UpdateHeadSubAccountInfoAsync(headAccountId, subAccountId))
                .Returns(Task.CompletedTask);
            mockAccountRelationshipRepository.Setup(x => x.CreateHeadSubAccountRelationshipAsync(headAccountId, subAccountId))
                .ReturnsAsync(accountRelationshipInfo);
            mockDomainEventFactory.Setup(x => x.CreateAccountLinkedEvent(accountRelationshipInfo, accountInfo.AccountName))
                .Returns(accountLinkedEvent);
            mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            KafkaProducedEventDto? capturedEvent = null;
            mockOutboxRepository.Setup(x => x.AddKafkaProducedEventAsync(It.IsAny<KafkaProducedEventDto>(), It.IsAny<CancellationToken>()))
                .Callback<KafkaProducedEventDto, CancellationToken>((evt, ct) => capturedEvent = evt)
                .Returns(Task.CompletedTask);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedEvent);
            Assert.Equal(subAccountId, capturedEvent.AccountId);
            Assert.Equal(EventTypes.AccountLinked, capturedEvent.EventType);
        }

        /// <summary>
        /// Tests that the handler wraps and rethrows unexpected exceptions
        /// with appropriate logging when an error occurs during processing.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_UnexpectedException_LogsAndThrowsWrappedException()
        {
            // Arrange
            var command = new LinkSubAccountCommand
            {
                LinkSubAccountDto = new LinkSubAccountDto
                {
                    HeadAccountId = Guid.NewGuid(),
                    SubAccountId = Guid.NewGuid(),
                },
            };

            mockAccountRepository.Setup(x => x.CheckAccountExistsAsync("ASDFG"))
                .ThrowsAsync(new InvalidOperationException("Database connection failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => handler.Handle(command, CancellationToken.None));

            Assert.Equal("An unexpected error occurred while linking the sub-account.", exception.Message);
            Assert.IsType<InvalidOperationException>(exception.InnerException);
        }
    }
}
