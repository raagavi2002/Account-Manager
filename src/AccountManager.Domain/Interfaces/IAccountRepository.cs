// <copyright file="IAccountRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Interfaces
{
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Results;

    /// <summary>
    /// Defines a contract for account data persistence operations.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Creates a new account in the underlying data store.
        /// </summary>
        /// <param name="accountDto">
        /// A data transfer object containing the account information to be created.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous create operation.
        /// </returns>
        Task<CreateAccountResult> CreateAccountAsync(CreateAccountDto accountDto);

        /// <summary>
        /// Retrieves account information for the specified account identifier.
        /// </summary>
        /// <param name="accountId">
        /// The unique identifier of the account.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the account information as an <see cref="AccountDto"/>.
        /// </returns>
        Task<AccountDto?> GetAccountInfoByIdAsync(Guid accountId);

        /// <summary>
        /// Asynchronously updates the status of a specified account.
        /// </summary>
        /// <param name="accountId">
        /// The unique identifier of the account whose status needs to be updated.
        /// </param>
        /// <param name="accountStatus">
        /// The new status to assign to the account.
        /// This could represent values such as "Active", "Inactive".
        /// </param>
        /// <param name="isArchive">flag reprsenting is account is archive or not.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task will complete once the account status has been updated.
        /// </returns>
        Task UpdateAccountStatusAsync(Guid accountId, string accountStatus, bool isArchive = false);

        /// <summary>
        /// Checks whether an account with the specified name already exists in the system.
        /// </summary>
        /// <param name="accountName">The account name to check for existence.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains <c>true</c> if an account with the specified name exists; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="accountName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="accountName"/> is empty or whitespace.
        /// </exception>
        Task<bool> CheckAccountExistsAsync(string accountName);

        /// <summary>
        /// Checks whether an account with the specified name already exists in the system.
        /// </summary>
        /// <param name="accountId">The account id to check for existence.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains <c>true</c> if an account with the specified accountId exists; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="accountId"/> is null.
        /// </exception>
        Task<bool> CheckAccountExistsAsync(Guid accountId);

        /// <summary>
        /// Updates the head account and sub-account relationship information in the database context.
        /// </summary>
        /// <param name="headAccountId">The unique identifier of the head account.</param>
        /// <param name="subAccountId">The unique identifier of the sub-account to be linked.</param>
        /// <returns>representing the asynchronous operation.</returns>
        Task UpdateHeadSubAccountInfoAsync(Guid headAccountId, Guid subAccountId);

        /// <summary>
        /// Updates an existing account with the values provided in the account data transfer object.
        /// Only fields that contain values are updated; all other fields remain unchanged.
        /// </summary>
        /// <param name="accountDto">
        /// The data transfer object containing updated account information.
        /// Fields that are <c>null</c>, empty, or whitespace are ignored.</param>
        /// <returns>
        /// A <see cref="UpdateAccountResult"/> containing the account identifier,
        /// updated version, and audit information after the update.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// Thrown when an account with the specified identifier does not exist.
        /// </exception>
        Task<(UpdateAccountResult, List<FieldChangeDto>)> UpdateAccountAsync(UpdateAccountDto accountDto);

        /// <summary>
        /// Retrieves the invoicing type for the specified account.
        /// </summary>
        /// <param name="accountId">
        /// The unique identifier of the account whose invoicing type is requested.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the invoicing type associated with the account,
        /// or an empty string if no invoicing type is defined.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// Thrown when an account with the specified <paramref name="accountId"/> does not exist.
        /// </exception>
        Task<string> GetInvoicingTypeAsync(Guid accountId);

        /// <summary>
        /// Retrieves all available time zones supported by the system.
        /// </summary>
        /// <remarks>
        /// This method queries the underlying data source or service to return a list of
        /// <see cref="TimezoneDto"/> objects, each representing a valid time zone.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation,
        /// with a result of type <see cref="List{T}"/> containing <see cref="TimezoneDto"/> entries.
        /// </returns>
        Task<List<TimezoneDto>> GetAllTimezonesAsync();

        /// <summary>
        /// Retrieves a timezone by its unique identifier.
        /// </summary>
        /// <param name="timezoneId">The unique identifier of the timezone.</param>
        /// <returns>
        /// A <see cref="TimezoneDto"/> containing the timezone ID and IANA name,
        /// or <c>null</c> if no matching timezone is found.
        /// </returns>
        Task<TimezoneDto?> GetTimezoneByIdAsync(int timezoneId);

        /// <summary>
        /// Retrieves products associated with an account using optional active-state filtering and pagination.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="isActive">Optional filter to return only active or inactive product associations.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="pageNumber">The page number starting at 1.</param>
        /// <returns>
        /// A tuple containing the paginated products and the total matching count before pagination.
        /// </returns>
        Task<(List<ProductAssociationDto> Products, int TotalCount)> GetAccountProductsAsync(
            Guid accountId,
            bool? isActive,
            int pageSize,
            int pageNumber);

        /// <summary>
        /// Asynchronously unlinks a sub-account from its head account.
        /// </summary>
        /// <param name="headAccountId">The unique identifier of the head account.</param>
        /// <param name="subAccountId">The unique identifier of the sub-account to be unlinked.</param>
        /// <returns>A task representing the asynchronous unlink operation.</returns>
        Task UnlinkSubAccountAsync(Guid headAccountId, Guid subAccountId);

        /// <summary>
        /// Checks whether an timezone with the specified name already exists in the system.
        /// </summary>
        /// <param name="timezone">The timezone to check for existence.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains <c>true</c> if an timezone with the specified name exists; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="timezone"/> is null.
        /// </exception>
        Task<bool> CheckTimezoneExistsAsync(string timezone);
    }
}
