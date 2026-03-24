using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FastEndpoints;
using AutoMapper;
using AccountManager.API.User.UpdateUser;
using AccountManager.Application.Commands.UpdateUserCommand;
using MediatR;
using FluentValidation;

namespace AccountManager.API.Tests.UpdateUserEndpointTests
{
    /// <summary>
    /// Unit tests for UpdateUserEndpoint.
    /// Tests the API endpoint behavior for updating user information.
    /// </summary>
    public class UpdateUserEndpointTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<UpdateUserRequest>> _mockValidator;
        private readonly UpdateUserEndpoint _endpoint;

        public UpdateUserEndpointTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMapper = new Mock<IMapper>();
            _mockValidator = new Mock<IValidator<UpdateUserRequest>>();
            _endpoint = new UpdateUserEndpoint(_mockMediator.Object, _mockMapper.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidRequest_ReturnsOkWithUpdatedData()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest
            {
                UserId = userId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
            };

            var commandResponse = new UpdateUserCommandResponse
            {
                UserId = userId,
                Email = "jane@example.com",
                UpdatedAt = DateTime.UtcNow,
            };

            var expectedResponse = new UpdateUserResponse
            {
                UserId = userId,
                Email = "jane@example.com",
                UpdatedAt = DateTime.UtcNow,
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResponse);

            _mockMapper.Setup(m => m.Map<UpdateUserResponse>(commandResponse))
                .Returns(expectedResponse);

            // Act
            await _endpoint.HandleAsync(request, CancellationToken.None);

            // Assert
            _mockMediator.Verify(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<UpdateUserResponse>(commandResponse), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithInvalidEmail_ThrowsValidationException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest
            {
                UserId = userId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "invalid-email",
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Invalid email format"));

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _endpoint.HandleAsync(request, CancellationToken.None));

            _mockMediator.Verify(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_WithEmptyFirstName_ThrowsValidationException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest
            {
                UserId = userId,
                FirstName = "",
                LastName = "Smith",
                Email = "jane@example.com",
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("FirstName", "First name is required"));

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _endpoint.HandleAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task HandleAsync_MapsCommandResponseToApiResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest
            {
                UserId = userId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
            };

            var commandResponse = new UpdateUserCommandResponse
            {
                UserId = userId,
                Email = "jane@example.com",
                UpdatedAt = DateTime.UtcNow,
            };

            var apiResponse = new UpdateUserResponse
            {
                UserId = userId,
                Email = "jane@example.com",
                UpdatedAt = DateTime.UtcNow,
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResponse);

            _mockMapper.Setup(m => m.Map<UpdateUserResponse>(commandResponse))
                .Returns(apiResponse);

            // Act
            await _endpoint.HandleAsync(request, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<UpdateUserResponse>(commandResponse), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithValidData_CallsMediator()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest
            {
                UserId = userId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
            };

            var commandResponse = new UpdateUserCommandResponse
            {
                UserId = userId,
                Email = "jane@example.com",
                UpdatedAt = DateTime.UtcNow,
            };

            var apiResponse = new UpdateUserResponse
            {
                UserId = userId,
                Email = "jane@example.com",
                UpdatedAt = DateTime.UtcNow,
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResponse);

            _mockMapper.Setup(m => m.Map<UpdateUserResponse>(commandResponse))
                .Returns(apiResponse);

            // Act
            await _endpoint.HandleAsync(request, CancellationToken.None);

            // Assert
            _mockMediator.Verify(m => m.Send(
                It.Is<UpdateUserCommand>(cmd =>
                    cmd.UserId == userId &&
                    cmd.FirstName == "Jane" &&
                    cmd.LastName == "Smith" &&
                    cmd.Email == "jane@example.com"),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
