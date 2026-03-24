using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FastEndpoints;
using AutoMapper;
using AccountManager.API.User.GetUser;
using AccountManager.Application.Queries.GetUserQuery;
using MediatR;

namespace AccountManager.API.Tests.GetUserEndpointTests
{
    /// <summary>
    /// Unit tests for GetUserEndpoint.
    /// Tests the API endpoint behavior for retrieving user profile.
    /// </summary>
    public class GetUserEndpointTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetUserEndpoint _endpoint;

        public GetUserEndpointTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMapper = new Mock<IMapper>();
            _endpoint = new GetUserEndpoint(_mockMediator.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidRequest_ReturnsOkWithUserData()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new GetUserRequest { UserId = userId };

            var queryResponse = new GetUserQueryResponse
            {
                UserId = userId,
                AccountId = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Roles = new List<string> { "MAIN_CLIENT" },
                IsActive = true,
                LoginCount = 5,
                CreatedAt = DateTime.UtcNow,
                Version = 1,
            };

            var expectedResponse = new GetUserResponse
            {
                UserId = userId,
                AccountId = queryResponse.AccountId,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Roles = new List<string> { "MAIN_CLIENT" },
                IsActive = true,
                LoginCount = 5,
                CreatedAt = DateTime.UtcNow,
                Version = 1,
            };

            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserQueryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResponse);

            _mockMapper.Setup(m => m.Map<GetUserResponse>(queryResponse))
                .Returns(expectedResponse);

            // Act
            await _endpoint.HandleAsync(request, CancellationToken.None);

            // Assert
            _mockMediator.Verify(m => m.Send(It.IsAny<GetUserQueryRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<GetUserResponse>(queryResponse), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithInvalidUserId_ResultsInQueryException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new GetUserRequest { UserId = userId };

            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserQueryRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _endpoint.HandleAsync(request, CancellationToken.None));

            _mockMediator.Verify(m => m.Send(It.IsAny<GetUserQueryRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<GetUserResponse>(It.IsAny<GetUserQueryResponse>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_MapsQueryResponseToApiResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new GetUserRequest { UserId = userId };

            var queryResponse = new GetUserQueryResponse
            {
                UserId = userId,
                Email = "mapped@example.com",
                FirstName = "Mapped",
                LastName = "User",
            };

            var apiResponse = new GetUserResponse
            {
                UserId = userId,
                Email = "mapped@example.com",
                FirstName = "Mapped",
                LastName = "User",
            };

            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserQueryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResponse);

            _mockMapper.Setup(m => m.Map<GetUserResponse>(queryResponse))
                .Returns(apiResponse);

            // Act
            await _endpoint.HandleAsync(request, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<GetUserResponse>(queryResponse), Times.Once);
        }
    }
}
