// <copyright file="GetAccountDetailsQueryHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Tests.Queries
{
    using AccountManager.Application.Queries.GetAccountDetailsQuery;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Shared.Logging;
    using Moq;

    /// <summary>
    /// Unit tests for <see cref="GetAccountDetailsQueryHandler"/>.
    /// </summary>
    public class GetAccountDetailsQueryHandlerTests
    {
        private readonly Mock<IAccountRepository> accountRepositoryMock;
        private readonly Mock<IApplogger> apploggerMock;
        private readonly GetAccountDetailsQueryHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountDetailsQueryHandlerTests"/> class
        /// and sets up the required mocks.
        /// </summary>
        public GetAccountDetailsQueryHandlerTests()
        {
            accountRepositoryMock = new Mock<IAccountRepository>();
            apploggerMock = new Mock<IApplogger>();

            handler = new GetAccountDetailsQueryHandler(
                accountRepositoryMock.Object,
                apploggerMock.Object);
        }

        /// <summary>
        /// Verifies that the handler returns a fully mapped
        /// <see cref="GetAccountDetailsQueryResponse"/> when the account exists.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_WhenAccountExists_ReturnsMappedResponse()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            var accountInfo = new AccountDto
            {
                AccountId = accountId,
                AccountName = "Test Account",
                AccountType = "Enterprise",
                Currency = "USD",
                Timezone = "UTC",
                Address = new AddressDto
                {
                    Street = "123 Test St",
                    City = "Test City",
                    State = "TS",
                    PostalCode = "12345",
                    Country = "Test Country",
                },
                VatNumber = "VAT123",
                AccountManagerId = Guid.NewGuid(),
                CsmId = Guid.NewGuid(),
                HeadAccountId = Guid.NewGuid(),
                InvoiceEmailAddress = "invoice@test.com",
                InvoiceType = "Monthly",
                NotificationEmailAddress = "notify@test.com",
                AccountStatus = "Active",
                Version = 3,
                IsHeadAccount = true,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow,
                ActivatedAt = DateTime.UtcNow.AddDays(-9),
                DeactivatedAt = null,
            };

            accountRepositoryMock
                .Setup(r => r.GetAccountInfoByIdAsync(accountId))
                .ReturnsAsync(accountInfo);

            var request = new GetAccountDetailsQueryRequest
            {
                AccountId = accountId,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountInfo.AccountId, result.AccountId);
            Assert.Equal(accountInfo.AccountName, result.AccountName);
            Assert.Equal(accountInfo.AccountType, result.AccountType);
            Assert.Equal(accountInfo.Currency, result.Currency);
            Assert.Equal(accountInfo.Timezone, result.Timezone);
            Assert.Equal(accountInfo.Address, result.Address);
            Assert.Equal(accountInfo.VatNumber, result.VatNumber);
            Assert.Equal(accountInfo.AccountManagerId, result.AccountManagerId);
            Assert.Equal(accountInfo.CsmId, result.CsmId);
            Assert.Equal(accountInfo.HeadAccountId, result.HeadAccountId);
            Assert.Equal(accountInfo.InvoiceEmailAddress, result.InvoiceEmailAddress);
            Assert.Equal(accountInfo.InvoiceType, result.InvoiceType);
            Assert.Equal(accountInfo.NotificationEmailAddress, result.NotificationEmailAddress);
            Assert.Equal(accountInfo.AccountStatus, result.AccountStatus);
            Assert.Equal(accountInfo.Version, result.Version);
            Assert.Equal(accountInfo.IsHeadAccount, result.IsHeadAccount);
            Assert.Equal(accountInfo.CreatedAt, result.CreatedAt);
            Assert.Equal(accountInfo.UpdatedAt, result.UpdatedAt);
            Assert.Equal(accountInfo.ActivatedAt, result.ActivatedAt);
            Assert.Equal(accountInfo.DeactivatedAt, result.DeactivatedAt);

            accountRepositoryMock.Verify(
                r => r.GetAccountInfoByIdAsync(accountId),
                Times.Once);
        }
    }
}
