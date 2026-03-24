// <copyright file="ListAccountUsersQueryHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Queries
{
    using AccountManager.Application.Queries.ListAccountUsersQuery;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using Moq;

    /// <summary>
    /// Unit tests for <see cref="ListAccountUsersQueryHandler"/>.
    /// </summary>
    public class ListAccountUsersQueryHandlerTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IApplogger> apploggerMock;
        private readonly ListAccountUsersQueryHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListAccountUsersQueryHandlerTests"/> class.
        /// </summary>
        public ListAccountUsersQueryHandlerTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            apploggerMock = new Mock<IApplogger>();
            handler = new ListAccountUsersQueryHandler(userRepositoryMock.Object, apploggerMock.Object);
        }

        /// <summary>
        /// Verifies that handler returns mapped users and pagination metadata.
        /// </summary>
        [Fact]
        public async Task Handle_WhenUsersExist_ReturnsPagedResponse()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var users = new List<UserDto>
            {
                new UserDto
                {
                    UserId = Guid.NewGuid(),
                    Email = "test@example.com",
                    FirstName = "Jane",
                    LastName = "Doe",
                    IsActive = true,
                    Roles = new List<string> { "MAIN_CLIENT" },
                },
            };

            userRepositoryMock
                .Setup(r => r.GetAccountUsersAsync(accountId, true, "MAIN_CLIENT", 20, 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((users, 5));

            var request = new ListAccountUsersQueryRequest
            {
                AccountId = accountId,
                IsActive = true,
                Role = "MAIN_CLIENT",
                PageSize = 20,
                PageNumber = 1,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Users);
            Assert.Equal(5, result.TotalCount);
            Assert.Equal(1, result.Page);
            Assert.Equal(20, result.PageSize);
            Assert.True(result.HasMore);

            userRepositoryMock.Verify(
                r => r.GetAccountUsersAsync(accountId, true, "MAIN_CLIENT", 20, 1, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that invalid pagination inputs are normalized to defaults.
        /// </summary>
        [Fact]
        public async Task Handle_WhenPagingValuesInvalid_NormalizesPaging()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            userRepositoryMock
                .Setup(r => r.GetAccountUsersAsync(accountId, null, null, 20, 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((new List<UserDto>(), 0));

            var request = new ListAccountUsersQueryRequest
            {
                AccountId = accountId,
                PageSize = 0,
                PageNumber = -5,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Users);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(1, result.Page);
            Assert.Equal(20, result.PageSize);
            Assert.False(result.HasMore);

            userRepositoryMock.Verify(
                r => r.GetAccountUsersAsync(accountId, null, null, 20, 1, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
