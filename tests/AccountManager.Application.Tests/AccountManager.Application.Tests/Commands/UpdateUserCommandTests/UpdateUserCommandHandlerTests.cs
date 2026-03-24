// <copyright file="UpdateUserCommandHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Commands.UpdateUserCommandTests
{
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Commands.UpdateUserCommand;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using Moq;

    /// <summary>
    /// Unit tests for <see cref="UpdateUserCommandHandler"/>.
    /// Tests both success and error scenarios for updating user information.
    /// </summary>
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<IApplogger> mockApplogger;
        private readonly Mock<IDomainEventFactory> mockDomainEventFactory;
        private readonly UpdateUserCommandHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserCommandHandlerTests"/> class.
        /// Sets up mocked dependencies and the handler under test.
        /// </summary>
        public UpdateUserCommandHandlerTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUserRepository = new Mock<IUserRepository>();
            mockApplogger = new Mock<IApplogger>();
            mockDomainEventFactory = new Mock<IDomainEventFactory>();

            mockUnitOfWork.Setup(u => u.User).Returns(mockUserRepository.Object);

            handler = new UpdateUserCommandHandler(mockUnitOfWork.Object, mockApplogger.Object, mockDomainEventFactory.Object);
        }

        /// <summary>
        /// Verifies that a valid request updates the user successfully
        /// and persists changes to the database.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WithValidRequest_UpdatesUserSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var requestorId = Guid.NewGuid();

            var command = new UpdateUserCommand
            {
                UserId = userId,
                RequestorId = requestorId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
            };

            var existingUser = new UserDto
            {
                UserId = userId,
                AccountId = accountId,
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                Version = 1,
            };

            mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            mockUserRepository.Setup(r => r.EmailExistsAsync("jane@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("jane@example.com", result.Email);
            Assert.NotEqual(default(DateTime), result.UpdatedAt);

            mockUserRepository.Verify(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            mockUserRepository.Verify(r => r.EmailExistsAsync("jane@example.com", It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that an invalid email format throws a <see cref="UserValidationException"/>.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WithInvalidEmail_ThrowsUserValidationException()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                RequestorId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "invalid-email",
            };

            // Act & Assert
            await Assert.ThrowsAsync<UserValidationException>(() => handler.Handle(command, CancellationToken.None));

            mockUserRepository.Verify(r => r.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        /// <summary>
        /// Verifies that an empty first name throws a <see cref="UserValidationException"/>.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WithEmptyFirstName_ThrowsUserValidationException()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                RequestorId = Guid.NewGuid(),
                FirstName = "",
                LastName = "Smith",
                Email = "jane@example.com",
            };

            // Act & Assert
            await Assert.ThrowsAsync<UserValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that an empty last name throws a <see cref="UserValidationException"/>.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WithEmptyLastName_ThrowsUserValidationException()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                UserId = Guid.NewGuid(),
                RequestorId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "",
                Email = "jane@example.com",
            };

            // Act & Assert
            await Assert.ThrowsAsync<UserValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        /// <summary>
        /// Verifies that a non-existent user throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WithNonExistentUser_ThrowsUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                UserId = userId,
                RequestorId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
            };

            mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => handler.Handle(command, CancellationToken.None));

            mockUserRepository.Verify(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that attempting to update with a duplicate email throws a <see cref="UserValidationException"/>.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WithDuplicateEmail_ThrowsUserValidationException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var command = new UpdateUserCommand
            {
                UserId = userId,
                RequestorId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "existing@example.com",
            };

            var existingUser = new UserDto
            {
                UserId = userId,
                AccountId = Guid.NewGuid(),
                Email = "jane@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                Version = 1,
            };

            var anotherUser = new UserDto
            {
                UserId = otherUserId,
                AccountId = Guid.NewGuid(),
                Email = "existing@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                IsActive = true,
                Version = 1,
            };

            mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            mockUserRepository.Setup(r => r.EmailExistsAsync("existing@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<UserValidationException>(() => handler.Handle(command, CancellationToken.None));

            mockUserRepository.Verify(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            mockUserRepository.Verify(r => r.EmailExistsAsync("existing@example.com", It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
