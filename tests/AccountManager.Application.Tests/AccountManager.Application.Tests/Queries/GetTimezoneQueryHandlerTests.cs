// <copyright file="GetTimezoneQueryHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Queries
{
    using AccountManager.Application.Queries.GetTimezoneQuery;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Interfaces;
    using Moq;

    /// <summary>
    /// Unit tests for <see cref="GetTimezoneQueryHandler"/>.
    /// </summary>
    public class GetTimezoneQueryHandlerTests
    {
        private readonly Mock<IAccountRepository> accountRepositoryMock;
        private readonly GetTimezoneQueryHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTimezoneQueryHandlerTests"/> class
        /// and sets up required dependencies.
        /// </summary>
        public GetTimezoneQueryHandlerTests()
        {
            accountRepositoryMock = new Mock<IAccountRepository>();
            handler = new GetTimezoneQueryHandler(accountRepositoryMock.Object);
        }

        /// <summary>
        /// Ensures that the handler returns a populated list of timezones
        /// when the repository provides available timezones.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WhenTimezonesExist_ReturnsTimezoneList()
        {
            // Arrange
            var expectedTimezones = new List<TimezoneDto>
            {
                new TimezoneDto { Id = 1, Name = "UTC" },
                new TimezoneDto { Id = 2, Name = "PST" },
                new TimezoneDto { Id = 3, Name = "EST" },
            };

            accountRepositoryMock.Setup(r => r.GetAllTimezonesAsync()).ReturnsAsync(expectedTimezones);

            var request = new GetTimezoneQueryRequest();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Timezones);
            Assert.Equal(expectedTimezones, result.Timezones);

            accountRepositoryMock.Verify(
                r => r.GetAllTimezonesAsync(),
                Times.Once);
        }

        /// <summary>
        /// Ensures that the handler returns an empty timezone list
        /// when the repository returns no timezones.
        /// </summary>
        /// <returns> Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WhenNoTimezonesExist_ReturnsEmptyList()
        {
            // Arrange
            accountRepositoryMock
                .Setup(r => r.GetAllTimezonesAsync())
                .ReturnsAsync(new List<TimezoneDto>());

            var request = new GetTimezoneQueryRequest();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Timezones);
            Assert.Empty(result.Timezones);

            accountRepositoryMock.Verify(
                r => r.GetAllTimezonesAsync(),
                Times.Once);
        }
    }
}
