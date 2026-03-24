// <copyright file="GetAccountProductsQueryHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Queries
{
    using AccountManager.Application.Queries.GetAccountProductsQuery;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using Moq;

    /// <summary>
    /// Unit tests for <see cref="GetAccountProductsQueryHandler"/>.
    /// </summary>
    public class GetAccountProductsQueryHandlerTests
    {
        private readonly Mock<IAccountRepository> accountRepositoryMock;
        private readonly Mock<IApplogger> apploggerMock;
        private readonly GetAccountProductsQueryHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountProductsQueryHandlerTests"/> class.
        /// </summary>
        public GetAccountProductsQueryHandlerTests()
        {
            accountRepositoryMock = new Mock<IAccountRepository>();
            apploggerMock = new Mock<IApplogger>();
            handler = new GetAccountProductsQueryHandler(accountRepositoryMock.Object, apploggerMock.Object);
        }

        /// <summary>
        /// Verifies that handler returns mapped products and pagination metadata.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task Handle_WhenProductsExist_ReturnsPagedResponse()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            List<ProductAssociationDto> products =
            [
                new ProductAssociationDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Alpha Product",
                    IsActive = true,
                    LastSyncedAt = DateTime.UtcNow.AddMinutes(-10),
                },
            ];

            accountRepositoryMock
                .Setup(r => r.GetAccountProductsAsync(accountId, true, 20, 1))
                .ReturnsAsync((products, 25));

            var request = new GetAccountProductsQueryRequest
            {
                AccountId = accountId,
                IsActive = true,
                PageSize = 20,
                PageNumber = 1,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Products);
            Assert.Equal(25, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(20, result.PageSize);
            Assert.True(result.HasMore);

            accountRepositoryMock.Verify(
                r => r.GetAccountProductsAsync(accountId, true, 20, 1),
                Times.Once);
        }

        /// <summary>
        /// Verifies that invalid pagination inputs are normalized to defaults.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task Handle_WhenPagingValuesInvalid_NormalizesPaging()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            accountRepositoryMock
                .Setup(r => r.GetAccountProductsAsync(accountId, null, 20, 1))
                .ReturnsAsync((new List<ProductAssociationDto>(), 0));

            var request = new GetAccountProductsQueryRequest
            {
                AccountId = accountId,
                PageSize = 0,
                PageNumber = -2,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Products);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(20, result.PageSize);
            Assert.False(result.HasMore);

            accountRepositoryMock.Verify(
                r => r.GetAccountProductsAsync(accountId, null, 20, 1),
                Times.Once);
        }
    }
}
