// <copyright file="UnlinkSubAccountCommandHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Commands.UnlinkSubAccountCommand;
    using AccountManager.Application.DTO;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="UnlinkSubAccountCommandHandler"/>.
    /// </summary>
    public class UnlinkSubAccountCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> accountRepositoryMock = new ();
        private readonly Mock<IAccountRelationshipRepository> accountRelationshipRepositoryMock = new ();
        private readonly Mock<IDomainEventFactory> domainEventFactoryMock = new ();
        private readonly Mock<IUnitOfWork> unitOfWorkMock = new ();

        /// <summary>
        /// Verifies that a valid unlink request unlinks the sub-account and returns a successful response.
        /// </summary>
        /// <returns> Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_ValidRequest_ShouldUnlinkSubAccountAndReturnResponse()
        {
            // Arrange
            var headAccountId = Guid.NewGuid();
            var subAccountId = Guid.NewGuid();

            var command = new UnlinkSubAccountCommand
            {
                UnlinkAccountInfo = new UnlinkSubAccountDto
                {
                    HeadAccountId = Guid.NewGuid(),
                    SubAccountId = Guid.NewGuid(),
                    Reason = "No longer needed",
                },
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(headAccountId))
                .ReturnsAsync(true);

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(subAccountId))
                .ReturnsAsync(true);

            var relationship = new AccountRelationshipDto
            {
                HeadAccountId = headAccountId,
                SubAccountId = subAccountId,
            };

            accountRelationshipRepositoryMock
                .Setup(r => r.GetAccountRelationshipAsync(headAccountId, subAccountId))
                .ReturnsAsync(relationship);

            var unlinkInfo = new Domain.Results.UnlinkSubAccountResult
            {
                UnlinkedAt = DateTime.UtcNow,
                UnlinkedBy = Guid.NewGuid(),
                Reason = "Manual unlink",
            };

            accountRelationshipRepositoryMock
                .Setup(r => r.UnlinkSubAccountAsync(command.UnlinkAccountInfo))
                .ReturnsAsync(unlinkInfo);

            var handler = CreateHandler();

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(subAccountId, response.SubAccountId);
            Assert.Equal(headAccountId, response.FormerHeadAccountId);
            Assert.Equal(unlinkInfo.UnlinkedAt, response.UnlinkedAt);
            Assert.Equal(unlinkInfo.UnlinkedBy, response.UnlinkedBy);
            Assert.Equal(unlinkInfo.Reason, response.Reason);

            accountRepositoryMock.Verify(
                r => r.UnlinkSubAccountAsync(headAccountId, subAccountId),
                Times.Once);

            unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that an exception is thrown when the head account does not exist.
        /// </summary>
        /// <returns> Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WhenHeadAccountDoesNotExist_ShouldThrowAccountAlreadyExistsException()
        {
            // Arrange
            var command = new UnlinkSubAccountCommand
            {
                UnlinkAccountInfo = new UnlinkSubAccountDto
                {
                    HeadAccountId = Guid.NewGuid(),
                    SubAccountId = Guid.NewGuid(),
                    Reason = "No longer needed",
                },
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(command.UnlinkAccountInfo.HeadAccountId))
                .ReturnsAsync(false);

            var handler = CreateHandler();

            // Act & Assert
            await Assert.ThrowsAsync<AccountAlreadyExistsException>(
                () => handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that an exception is thrown when the sub-account does not exist.
        /// </summary>
        /// <returns> Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WhenSubAccountDoesNotExist_ShouldThrowAccountAlreadyExistsException()
        {
            // Arrange
            var command = new UnlinkSubAccountCommand
            {
                UnlinkAccountInfo = new UnlinkSubAccountDto
                {
                    HeadAccountId = Guid.NewGuid(),
                    SubAccountId = Guid.NewGuid(),
                    Reason = "No longer needed",
                },
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(command.UnlinkAccountInfo.HeadAccountId))
                .ReturnsAsync(true);

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(command.UnlinkAccountInfo.SubAccountId))
                .ReturnsAsync(false);

            var handler = CreateHandler();

            // Act & Assert
            await Assert.ThrowsAsync<AccountAlreadyExistsException>(
                () => handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that an exception is thrown when no account relationship exists.
        /// </summary>
        /// <returns> Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WhenRelationshipNotFound_ShouldThrowAccountRelationshipNotFoundException()
        {
            // Arrange
            var command = new UnlinkSubAccountCommand
            {
                UnlinkAccountInfo = new UnlinkSubAccountDto
                {
                    HeadAccountId = Guid.NewGuid(),
                    SubAccountId = Guid.NewGuid(),
                    Reason = "No longer needed",
                },
            };

            accountRepositoryMock
                .Setup(r => r.CheckAccountExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            accountRelationshipRepositoryMock
                .Setup(r => r.GetAccountRelationshipAsync(
                    command.UnlinkAccountInfo.HeadAccountId,
                    command.UnlinkAccountInfo.SubAccountId))
                .ReturnsAsync((AccountRelationshipDto?)null);

            var handler = CreateHandler();

            // Act & Assert
            await Assert.ThrowsAsync<AccountRelationshipNotFoundException>(
                () => handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Creates a new instance of <see cref="UnlinkSubAccountCommandHandler"/> with mocked dependencies.
        /// </summary>
        /// <returns> Task representing the asynchronous unit test.</returns>
        private UnlinkSubAccountCommandHandler CreateHandler()
        {
            return new UnlinkSubAccountCommandHandler(
                accountRepositoryMock.Object,
                accountRelationshipRepositoryMock.Object,
                domainEventFactoryMock.Object,
                unitOfWorkMock.Object);
        }
    }
}
