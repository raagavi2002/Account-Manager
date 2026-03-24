// <copyright file="GetUserQueryHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Queries.GetUserQueryTests
{
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Queries.GetUserQuery;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using Moq;

    /// <summary>
    /// Unit tests for <see cref="GetUserQueryHandler"/>.
    /// Tests both success and error scenarios for retrieving user profile information.
    /// </summary>
    public class GetUserQueryHandlerTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<IPermissionCalculator> mockPermissionCalculator;
        private readonly GetUserQueryHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserQueryHandlerTests"/> class.
        /// Sets up mocked dependencies and the handler under test.
        /// </summary>
        public GetUserQueryHandlerTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockPermissionCalculator = new Mock<IPermissionCalculator>();
            handler = new GetUserQueryHandler(mockUserRepository.Object, mockPermissionCalculator.Object);
        }

        /// <summary>
        /// Verifies that a valid user ID returns a populated <see cref="GetUserQueryResponse"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="GetUserQueryResponse"/> containing:
        /// <list type="bullet">
        /// <item><description>UserId matching the request.</description></item>
        /// <item><description>AccountId of the user.</description></item>
        /// <item><description>Email, FirstName, LastName, Roles, IsActive, LastLoginAt, and LoginCount.</description></item>
        /// </list>
        /// </returns>
        [Fact]
        public async Task Handle_WithValidUserId_ReturnsUserProfile()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new GetUserQueryRequest { UserId = userId, RequestorId = Guid.NewGuid() };

            var user = new UserDto
            {
                UserId = userId,
                AccountId = accountId,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                LastLoginAt = DateTime.UtcNow,
                LoginCount = 5,
                Version = 1,
                Roles = new List<string> { "MAIN_CLIENT" },
            };

            mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(accountId, result.AccountId);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.True(result.IsActive);
            Assert.Equal(5, result.LoginCount);
            Assert.Contains("MAIN_CLIENT", result.Roles);

            mockUserRepository.Verify(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that an invalid user ID throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <returns>
        /// An exception of type <see cref="UserNotFoundException"/> indicating the user does not exist.
        /// </returns>
        [Fact]
        public async Task Handle_WithInvalidUserId_ThrowsUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new GetUserQueryRequest { UserId = userId, RequestorId = Guid.NewGuid() };

            mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => handler.Handle(request, CancellationToken.None));

            mockUserRepository.Verify(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that when a valid user is retrieved, permissions are calculated
        /// and returned in the <see cref="GetUserQueryResponse"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="GetUserQueryResponse"/> containing the user's profile and calculated permissions.
        /// Example: <c>{ Read = true, Write = false }</c>.
        /// </returns>
        [Fact]
        public async Task Handle_WithValidUser_CalculatesPermissions()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new GetUserQueryRequest { UserId = userId, RequestorId = Guid.NewGuid() };

            var user = new UserDto
            {
                UserId = userId,
                AccountId = accountId,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsActive = true,
                Version = 1,
                Roles = new List<string> { "MAIN_CLIENT" },
            };

            var expectedPermissions = new { Read = true, Write = false };

            mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(expectedPermissions, result.Permissions);
        }
    }
}
