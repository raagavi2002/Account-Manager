/// <copyright file="AddUserCommandHandlerTests.cs" company="PlaceholderCompany">
/// Copyright (c) PlaceholderCompany. All rights reserved.
/// </copyright>

namespace AccountManager.Application.Tests.Commands.AddUserCommand
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Application.Abstractions;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Application.Authorization;
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Commands.AddUserCommand;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Domain.Results;
    using AccountManager.Shared.Logging;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="AddUserCommandHandler"/>.
    /// All repository interactions are driven exclusively through <see cref="IUnitOfWork"/>
    /// using Moq's recursive mock chaining on <c>unitOfWork.User</c>.
    /// </summary>
    public class AddUserCommandHandlerTests
    {
        private readonly Mock<IDomainEventFactory> _domainEventFactoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IApplogger> _apploggerMock;
        private readonly Mock<IPermissionCalculator> _permissionCalculatorMock;
        private readonly Mock<IClerkService> _clerkService;
        private readonly AddUserCommandHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserCommandHandlerTests"/> class.
        /// Sets up shared mocks and the system under test. The <see cref="IUnitOfWork"/> mock
        /// is created with <c>MockBehavior.Default</c> so that <c>unitOfWork.User</c> is
        /// auto-mocked as an interface and supports recursive setup chaining.
        /// </summary>
        public AddUserCommandHandlerTests()
        {
            _domainEventFactoryMock = new Mock<IDomainEventFactory>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _apploggerMock = new Mock<IApplogger>();
            _permissionCalculatorMock = new Mock<IPermissionCalculator>();
            _clerkService = new Mock<IClerkService>();

            _handler = new AddUserCommandHandler(
                _domainEventFactoryMock.Object,
                _unitOfWorkMock.Object,
                _apploggerMock.Object,
                _permissionCalculatorMock.Object, _clerkService.Object);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidEmail"
        /// is thrown when the email address is null.
        /// </summary>
        [Fact]
        public async Task Handle_WhenEmailIsNull_ThrowsUserValidationExceptionWithInvalidEmailCode()
        {
            // Arrange
            var command = BuildCommand(email: null);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidEmail", exception.Error.Code);
            Assert.Equal("Email address is required.", exception.Error.Message);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidEmail"
        /// is thrown when the email address is an empty string.
        /// </summary>
        [Fact]
        public async Task Handle_WhenEmailIsEmpty_ThrowsUserValidationExceptionWithInvalidEmailCode()
        {
            // Arrange
            var command = BuildCommand(email: string.Empty);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidEmail", exception.Error.Code);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidFirstName"
        /// is thrown when the first name is null.
        /// </summary>
        [Fact]
        public async Task Handle_WhenFirstNameIsNull_ThrowsUserValidationExceptionWithInvalidFirstNameCode()
        {
            // Arrange
            var command = BuildCommand(firstName: null);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidFirstName", exception.Error.Code);
            Assert.Equal("First name is required.", exception.Error.Message);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidFirstName"
        /// is thrown when the first name is an empty string.
        /// </summary>
        [Fact]
        public async Task Handle_WhenFirstNameIsEmpty_ThrowsUserValidationExceptionWithInvalidFirstNameCode()
        {
            // Arrange
            var command = BuildCommand(firstName: string.Empty);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidFirstName", exception.Error.Code);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidLastName"
        /// is thrown when the last name is null.
        /// </summary>
        [Fact]
        public async Task Handle_WhenLastNameIsNull_ThrowsUserValidationExceptionWithInvalidLastNameCode()
        {
            // Arrange
            var command = BuildCommand(lastName: null);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidLastName", exception.Error.Code);
            Assert.Equal("Last name is required.", exception.Error.Message);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidLastName"
        /// is thrown when the last name is an empty string.
        /// </summary>
        [Fact]
        public async Task Handle_WhenLastNameIsEmpty_ThrowsUserValidationExceptionWithInvalidLastNameCode()
        {
            // Arrange
            var command = BuildCommand(lastName: string.Empty);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidLastName", exception.Error.Code);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "EmailAlreadyExists"
        /// is thrown when <see cref="IUnitOfWork.User"/> reports the email address is already taken.
        /// </summary>
        [Fact]
        public async Task Handle_WhenEmailAlreadyExists_ThrowsUserValidationExceptionWithEmailAlreadyExistsCode()
        {
            // Arrange
            var command = BuildCommand();
            _unitOfWorkMock
                .Setup(u => u.User.EmailExistsAsync(command.AddUser.EmailAddress, CancellationToken.None))
                .ReturnsAsync(true);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("EmailAlreadyExists", exception.Error.Code);
            Assert.Equal("A user with the same email address already exists.", exception.Error.Message);
            Assert.True(exception.Error.Details.AdditionalInfo.ContainsKey("EmailAddress"));
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "NameAlreadyExists"
        /// is thrown when <see cref="IUnitOfWork.User"/> reports the full name is already taken.
        /// </summary>
        [Fact]
        public async Task Handle_WhenNameAlreadyExists_ThrowsUserValidationExceptionWithNameAlreadyExistsCode()
        {
            // Arrange
            CancellationToken cancellationToken = CancellationToken.None;
            var command = BuildCommand();
            _unitOfWorkMock
                .Setup(u => u.User.EmailExistsAsync(command.AddUser.EmailAddress, cancellationToken))
                .ReturnsAsync(false);
            _unitOfWorkMock
                .Setup(u => u.User.DuplicateNameExistsAsync(command.AddUser.FirstName, command.AddUser.LastName, null, cancellationToken))
                .ReturnsAsync(true);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("NameAlreadyExists", exception.Error.Code);
            Assert.Equal("A user with the same name already exists.", exception.Error.Message);
            Assert.True(exception.Error.Details.AdditionalInfo.ContainsKey("First Name"));
            Assert.True(exception.Error.Details.AdditionalInfo.ContainsKey("Last Name"));
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidRoles"
        /// is thrown when the roles collection is null.
        /// </summary>
        [Fact]
        public async Task Handle_WhenRolesIsNull_ThrowsUserValidationExceptionWithInvalidRolesCode()
        {
            // Arrange
            var command = BuildCommand(roles: null);
            SetupNoDuplicates(command);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidRoles", exception.Error.Code);
            Assert.Equal("At least one role is required.", exception.Error.Message);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidRoles"
        /// is thrown when the roles collection is empty.
        /// </summary>
        [Fact]
        public async Task Handle_WhenRolesIsEmpty_ThrowsUserValidationExceptionWithInvalidRolesCode()
        {
            // Arrange
            var command = BuildCommand(roles: new List<string>());
            SetupNoDuplicates(command);

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidRoles", exception.Error.Code);
        }

        /// <summary>
        /// Verifies that a <see cref="UserValidationException"/> with code "InvalidRolesCombination"
        /// is thrown when the permission calculator deems the supplied roles invalid.
        /// </summary>
        [Fact]
        public async Task Handle_WhenRolesCombinationIsInvalid_ThrowsUserValidationExceptionWithInvalidRolesCombinationCode()
        {
            // Arrange
            var roles = new List<UserRoleType> { UserRoleType.Admin };
            var rles = new List<string> { "ADMIN" };
            var command = BuildCommand(roles: rles);
            SetupNoDuplicates(command);
            _permissionCalculatorMock
                .Setup(p => p.Validate(roles, Guid.Empty))
                .Returns(new RoleValidationResult
                {
                    IsValid = false,
                    ValidationMessages = new Dictionary<string, string>
                    {
                        { "Role1", "Conflicts with Role2." },
                    },
                });

            // Act
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("InvalidRolesCombination", exception.Error.Code);
            Assert.Equal("Error in role combination found.", exception.Error.Message);
        }

        /// <summary>
        /// Verifies that when all inputs are valid, <see cref="AddUserCommandHandler.Handle"/> returns
        /// an <see cref="AddUserCommandResponse"/> populated with the data returned by
        /// <see cref="IUnitOfWork.User"/>.
        /// </summary>
        [Fact]
        public async Task Handle_WhenRequestIsValid_ReturnsPopulatedAddUserCommandResponse()
        {
            // Arrange
            var roles = new List<string> { "Admin" };
            var command = BuildCommand(roles: roles);
            var expectedUserId = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;

            List<UserRoleType> roleTypes = roles?
            .Select(r => EnumParser.TryParse<UserRoleType>(r, out var roleType) ? roleType : (UserRoleType?)null)
            .Where(r => r.HasValue)
            .Select(r => r!.Value)
            .ToList()
            ?? new List<UserRoleType> { UserRoleType.Admin };

            SetupNoDuplicates(command);
            SetupValidPermissions(roles);

            _unitOfWorkMock
                .Setup(u => u.User.AddUserAsync(command.AddUser))
                .ReturnsAsync(new AddUserResult
                {
                    UserId = expectedUserId,
                    FirstName = command.AddUser.FirstName,
                    LastName = command.AddUser.LastName,
                    Email = command.AddUser.EmailAddress,
                    CreatedAt = createdAt,
                    IsActive = true,
                    Roles = roleTypes,
                });

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedUserId, response.UserId);
            Assert.Equal(command.AddUser.FirstName, response.FirstName);
            Assert.Equal(command.AddUser.LastName, response.LastName);
            Assert.Equal(command.AddUser.EmailAddress, response.Email);
            Assert.Equal(createdAt, response.CreatedAt);
            Assert.True(response.IsActive);
        }

        /// <summary>
        /// Verifies that <see cref="IUnitOfWork.SaveChangesAsync"/> is called exactly once
        /// when the command completes successfully, ensuring the unit of work commits the transaction.
        /// </summary>
        [Fact]
        public async Task Handle_WhenRequestIsValid_CallsSaveChangesAsyncOnce()
        {
            // Arrange
            var roles = new List<string> { "Admin" };
            var command = BuildCommand(roles: roles);

            SetupNoDuplicates(command);
            SetupValidPermissions(roles);
            SetupAddUserAsync(command, roles);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Builds a valid <see cref="AddUserCommand"/> with optional property overrides.
        /// </summary>
        /// <param name="email">The email address to use. Defaults to a valid address.</param>
        /// <param name="firstName">The first name to use. Defaults to "John".</param>
        /// <param name="lastName">The last name to use. Defaults to "Doe".</param>
        /// <param name="roles">The roles list to use. Defaults to a single "Admin" role.</param>
        /// <returns>A configured <see cref="AddUserCommand"/>.</returns>
        private static AddUserCommand BuildCommand(
            string? email = "john.doe@example.com",
            string? firstName = "John",
            string? lastName = "Doe",
            List<string>? roles = null)
        {
            List<UserRoleType> roleTypes = roles?
                    .Select(r => EnumParser.TryParse<UserRoleType>(r, out var roleType) ? roleType : (UserRoleType?)null)
                    .Where(r => r.HasValue)
                    .Select(r => r!.Value)
                    .ToList()
                    ?? new List<UserRoleType> { UserRoleType.Admin };

            return new AddUserCommand
            {
                AddUser = new AddUserDto
                {
                    AccountId = Guid.NewGuid(),
                    EmailAddress = email!,
                    FirstName = firstName!,
                    LastName = lastName!,
                    Roles = roleTypes ?? new List<UserRoleType> { UserRoleType.Admin },
                },
            };
        }

        /// <summary>
        /// Configures <see cref="IUnitOfWork.User"/> via the unit of work mock to return
        /// no duplicate email or name matches for any input values.
        /// </summary>
        /// <param name="command">The command whose context is being stubbed.</param>
        private void SetupNoDuplicates(AddUserCommand command, CancellationToken cancellationToken = default)
        {
            _unitOfWorkMock
                .Setup(u => u.User.EmailExistsAsync(It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(false);

            _unitOfWorkMock
                .Setup(u => u.User.DuplicateNameExistsAsync(It.IsAny<string>(), It.IsAny<string>(), null, cancellationToken))
                .ReturnsAsync(false);
        }

        /// <summary>
        /// Configures the permission calculator mock to return a valid result for the given roles.
        /// </summary>
        /// <param name="roles">The roles to mark as a valid combination.</param>
        private void SetupValidPermissions(List<string> roles)
        {
            List<UserRoleType> roleTypes = roles?
                    .Select(r => EnumParser.TryParse<UserRoleType>(r, out var roleType) ? roleType : (UserRoleType?)null)
                    .Where(r => r.HasValue)
                    .Select(r => r!.Value)
                    .ToList()
                    ?? new List<UserRoleType> { UserRoleType.Admin };

            _permissionCalculatorMock
                .Setup(p => p.Validate(roleTypes, Guid.Empty))
                .Returns(new RoleValidationResult { IsValid = true });
        }

        /// <summary>
        /// Configures <see cref="IUnitOfWork.User.AddUserAsync"/> via the unit of work mock
        /// to return a minimal <see cref="UserInfo"/> derived from the supplied command.
        /// </summary>
        /// <param name="command">The command used to populate the returned user info.</param>
        /// <param name="roles">The roles to include in the returned user info.</param>
        private void SetupAddUserAsync(AddUserCommand command, List<string> roles)
        {
            List<UserRoleType> roleTypes = roles?
            .Select(r => EnumParser.TryParse<UserRoleType>(r, out var roleType) ? roleType : (UserRoleType?)null)
            .Where(r => r.HasValue)
            .Select(r => r!.Value)
            .ToList()
            ?? new List<UserRoleType> { UserRoleType.Admin };

            _unitOfWorkMock
                .Setup(u => u.User.AddUserAsync(command.AddUser))
                .ReturnsAsync(new Domain.Results.AddUserResult
                {
                    UserId = Guid.NewGuid(),
                    FirstName = command.AddUser.FirstName,
                    LastName = command.AddUser.LastName,
                    Email = command.AddUser.EmailAddress,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    Roles = roleTypes,
                });
        }
    }
}
