// <copyright file="AccountRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Repository
{
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Errors;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Domain.Results;
    using AccountManager.Infrastructure.Persistence.Entities;
    using Microsoft.EntityFrameworkCore;
    using Polly;

    /// <summary>
    /// Repository for managing account entities in the data store.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountManagerDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to be used by the repository.</param>
        public AccountRepository(AccountManagerDbContext context)
        {
            this.context = context;
        }

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
        public async Task<bool> CheckAccountExistsAsync(string accountName)
        {
            if (string.IsNullOrWhiteSpace(accountName))
            {
                throw new ArgumentException("Account name cannot be empty or whitespace.", nameof(accountName));
            }

            return await context.Accounts.AnyAsync(a => a.AccountName == accountName).ConfigureAwait(false);
        }

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
        public async Task<bool> CheckAccountExistsAsync(Guid accountId)
        {
            return await context.Accounts.AnyAsync(a => a.AccountId == accountId).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new account in the underlying data store.
        /// </summary>
        /// <param name="accountDto">
        /// A data transfer object containing the account information to be created.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous create operation.
        /// </returns>
        public async Task<CreateAccountResult> CreateAccountAsync(CreateAccountDto accountDto)
        {
            if (accountDto == null)
            {
                throw new ArgumentNullException(nameof(accountDto));
            }

            try
            {
                Account account = new Account
                {
                    AccountId = Guid.NewGuid(),
                    AccountName = accountDto?.AccountName ?? string.Empty,
                    AccountType = accountDto?.AccountType?.ToUpper() ?? string.Empty,
                    Currency = accountDto?.Currency?.ToUpper() ?? string.Empty,
                    Timezone = accountDto?.Timezone ?? string.Empty,
                    AccountStatus = EnumParser.GetEnumMemberValue<AccountStatus>(AccountStatus.Inactive),
                    AddressStreet = accountDto?.Address?.Street ?? string.Empty,
                    AddressStreet2 = accountDto?.Address?.Street2 ?? string.Empty,
                    AddressCity = accountDto?.Address?.City ?? string.Empty,
                    AddressState = accountDto?.Address?.State ?? string.Empty,
                    AddressPostalCode = accountDto?.Address?.PostalCode ?? string.Empty,
                    AddressCountry = accountDto?.Address?.Country ?? string.Empty,
                    VatNumber = accountDto?.VatNumber,
                    AccountManagerId = accountDto?.AccountManagerId,
                    CsmId = accountDto?.CsmId,
                    HeadAccountId = accountDto?.HeadAccountId,
                    DateFormat = "yyyy-mm-dd",
                    TimeFormat = "12h",
                    Locale = "en-US",
                    InvoiceEmailAddress = accountDto?.InvoiceEmailAddress,
                    InvoiceType = accountDto?.InvoiceType,
                    NotificationEmailAddress = accountDto?.NotificationEmailAddress,
                    IsActive = true,
                    IsHeadAccount = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system",
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = "system",
                    Version = 1,
                };

                await context.Accounts.AddAsync(account).ConfigureAwait(false);

                return new CreateAccountResult
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType,
                    AccountStatus = account.AccountStatus,
                    Currency = account.Currency,
                    Timezone = account.Timezone,
                    Version = account.Version,
                    CreatedAt = account.CreatedAt ?? DateTime.UtcNow,
                    UpdatedAt = account.UpdatedAt ?? DateTime.UtcNow,
                };
            }
            catch
            {
                throw;
            }
        }

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
        public async Task<AccountDto?> GetAccountInfoByIdAsync(Guid accountId)
        {
            var accountInfo = await context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.AccountId == accountId).ConfigureAwait(false);
            return accountInfo == null
                ? null
                : new AccountDto
                {
                    AccountId = accountInfo.AccountId,
                    IsHeadAccount = accountInfo.IsHeadAccount,
                    AccountStatus = accountInfo.AccountStatus,
                    HeadAccountId = accountInfo.HeadAccountId,
                    AccountName = accountInfo.AccountName,
                    AccountType = accountInfo.AccountType,
                    Currency = accountInfo.Currency,
                    Timezone = accountInfo.Timezone,
                    VatNumber = accountInfo.VatNumber,
                    AccountManagerId = accountInfo.AccountManagerId,
                    CsmId = accountInfo.CsmId,
                    Address = new AddressDto
                    {
                        Street = accountInfo.AddressStreet,
                        Street2 = accountInfo.AddressStreet2,
                        City = accountInfo.AddressCity,
                        State = accountInfo.AddressState,
                        PostalCode = accountInfo.AddressPostalCode,
                        Country = accountInfo.AddressCountry,
                    },
                    InvoiceEmailAddress = accountInfo.InvoiceEmailAddress,
                    InvoiceType = accountInfo.InvoiceType,
                    NotificationEmailAddress = accountInfo.NotificationEmailAddress,
                    Version = accountInfo.Version,
                    CreatedAt = accountInfo.CreatedAt,
                    UpdatedAt = accountInfo.UpdatedAt,
                    ActivatedAt = accountInfo.ActivatedAt,
                    DeactivatedAt = accountInfo.DeactivatedAt,
                };
        }

        /// <summary>
        /// Asynchronously updates the status of a specified account.
        /// </summary>
        /// <param name="accountId">
        /// The unique identifier of the account whose status needs to be updated.
        /// </param>
        /// <param name="accountStatus">
        /// The new status to assign to the account.
        /// This could represent values such as "Active", "Inactive", or "Suspended".
        /// </param>
        /// <param name="isArchive">flag representing the account is archived or not.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task will complete once the account status has been updated.
        /// </returns>
        /// <remarks>
        /// Use this method when you need to change the state of an account without blocking the calling thread.
        /// Ensure that the provided <paramref name="accountStatus"/> is valid within the system's domain rules.
        /// </remarks>
        public async Task UpdateAccountStatusAsync(Guid accountId, string accountStatus, bool isArchive = false)
        {
            try
            {
                var account = await context.Accounts.FirstOrDefaultAsync(acc => acc.AccountId == accountId);

                if (account != null && !isArchive)
                {
                    // Update the status
                    account.AccountStatus = accountStatus;
                    account.UpdatedAt = DateTime.UtcNow;
                    account.Version++;
                }

                if (account != null && isArchive)
                {
                    account.AccountStatus = accountStatus;
                    account.DeactivatedAt = DateTime.UtcNow;
                    account.IsActive = false;
                    account.Version++;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the account status.", ex);
            }
        }

        /// <summary>
        /// Updates the head account and sub-account relationship information in the database context.
        /// </summary>
        /// <param name="headAccountId">The unique identifier of the head account.</param>
        /// <param name="subAccountId">The unique identifier of the sub-account to be linked.</param>
        /// <returns>representing the asynchronous operation.</returns>
        public async Task UpdateHeadSubAccountInfoAsync(Guid headAccountId, Guid subAccountId)
        {
            var headAccountInfo = await context.Accounts.FirstOrDefaultAsync(account => account.AccountId == headAccountId);
            if (headAccountInfo != null)
            {
                headAccountInfo.IsHeadAccount = true;
                headAccountInfo.Version += 1;
                headAccountInfo.UpdatedAt = DateTime.UtcNow;
            }

            var subAccountInfo = await context.Accounts.FirstOrDefaultAsync(account => account.AccountId == subAccountId);
            if (subAccountInfo != null)
            {
                subAccountInfo.HeadAccountId = headAccountId;
                subAccountInfo.Version += 1;
                subAccountInfo.UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Updates an existing account with the values provided in the account data transfer object.
        /// Only fields that contain values are updated; all other fields remain unchanged.
        /// </summary>
        /// <param name="accountDto">
        /// The data transfer object containing updated account information.
        /// Fields that are <c>null</c>, empty, or whitespace are ignored.
        /// </param>
        /// <returns>
        /// A <see cref="UpdateAccountResult"/> containing the account identifier,
        /// updated version, and audit information after the update.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// Thrown when an account with the specified identifier does not exist.
        /// </exception>
        public async Task<(UpdateAccountResult, List<FieldChangeDto>)> UpdateAccountAsync(UpdateAccountDto accountDto)
        {
            var account = await context.Accounts.FirstOrDefaultAsync(acc => acc.AccountId == accountDto.AccountId);

            if (account == null)
            {
                throw new AccountNotFoundException(new ErrorResponses
                {
                    Code = "AccountNotFound",
                    Message = "Account not found.",
                    Details = new ErrorInfo
                    {
                        AccountId = accountDto.AccountId ?? Guid.Empty,
                    },
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            List<FieldChangeDto> changedFields = new List<FieldChangeDto>();

            // Update only if value is provided
            if (!string.IsNullOrWhiteSpace(accountDto.AccountName))
            {
                account.AccountName = accountDto.AccountName;
                changedFields.Add(new FieldChangeDto
                {
                    Field = "AccountName",
                    OldValue = account.AccountName,
                    NewValue = accountDto.AccountName,
                });
            }

            if (!string.IsNullOrWhiteSpace(accountDto.AccountType))
            {
                account.AccountType = accountDto.AccountType.ToUpperInvariant();
                changedFields.Add(new FieldChangeDto
                {
                    Field = "AccountType",
                    OldValue = account.AccountType,
                    NewValue = accountDto.AccountType.ToUpperInvariant(),
                });
            }

            if (!string.IsNullOrWhiteSpace(accountDto.Currency))
            {
                account.Currency = accountDto.Currency.ToUpperInvariant();
                changedFields.Add(new FieldChangeDto
                {
                    Field = "Currency",
                    OldValue = account.Currency,
                    NewValue = accountDto.Currency.ToUpperInvariant(),
                });
            }

            if (!string.IsNullOrWhiteSpace(accountDto.Timezone))
            {
                account.Timezone = accountDto.Timezone.ToUpperInvariant();
                changedFields.Add(new FieldChangeDto
                {
                    Field = "Timezone",
                    OldValue = account.Timezone,
                    NewValue = accountDto.Timezone.ToUpperInvariant(),
                });
            }

            if (accountDto.Address != null)
            {
                if (!string.IsNullOrWhiteSpace(accountDto.Address.Street))
                {
                    account.AddressStreet = accountDto.Address.Street;
                    changedFields.Add(new FieldChangeDto
                    {
                        Field = "AddressStreet",
                        OldValue = account.AddressStreet,
                        NewValue = accountDto.Address.Street,
                    });
                }

                if (!string.IsNullOrWhiteSpace(accountDto.Address.Street2))
                {
                    account.AddressStreet2 = accountDto.Address.Street2;
                    changedFields.Add(new FieldChangeDto
                    {
                        Field = "AddressStreet2",
                        OldValue = account.AddressStreet2,
                        NewValue = accountDto.Address.Street2,
                    });
                }

                if (!string.IsNullOrWhiteSpace(accountDto.Address.City))
                {
                    account.AddressCity = accountDto.Address.City;
                    changedFields.Add(new FieldChangeDto
                    {
                        Field = "AddressCity",
                        OldValue = account.AddressCity,
                        NewValue = accountDto.Address.City,
                    });
                }

                if (!string.IsNullOrWhiteSpace(accountDto.Address.State))
                {
                    account.AddressState = accountDto.Address.State;
                    changedFields.Add(new FieldChangeDto
                    {
                        Field = "AddressState",
                        OldValue = account.AddressState,
                        NewValue = accountDto.Address.State,
                    });
                }

                if (!string.IsNullOrWhiteSpace(accountDto.Address.PostalCode))
                {
                    account.AddressPostalCode = accountDto.Address.PostalCode;
                    changedFields.Add(new FieldChangeDto
                    {
                        Field = "AddressPostalCode",
                        OldValue = account.AddressPostalCode,
                        NewValue = accountDto.Address.PostalCode,
                    });
                }

                if (!string.IsNullOrWhiteSpace(accountDto.Address.Country))
                {
                    account.AddressCountry = accountDto.Address.Country;
                    changedFields.Add(new FieldChangeDto
                    {
                        Field = "AddressCountry",
                        OldValue = account.AddressCountry,
                        NewValue = accountDto.Address.Country,
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(accountDto.VatNumber))
            {
                account.VatNumber = accountDto.VatNumber;
                changedFields.Add(new FieldChangeDto
                {
                    Field = "VatNumber",
                    OldValue = account.VatNumber,
                    NewValue = accountDto.VatNumber,
                });
            }

            if (accountDto.AccountManagerId.HasValue)
            {
                account.AccountManagerId = accountDto.AccountManagerId.Value;
                changedFields.Add(new FieldChangeDto
                {
                    Field = "AccountManagerId",
                    OldValue = account.AccountManagerId.ToString() ?? string.Empty,
                    NewValue = accountDto.AccountManagerId.Value.ToString(),
                });
            }

            if (accountDto.CsmId.HasValue)
            {
                account.CsmId = accountDto.CsmId.Value;
                changedFields.Add(new FieldChangeDto
                {
                    Field = "CsmId",
                    OldValue = account.CsmId.ToString() ?? string.Empty,
                    NewValue = accountDto.CsmId.Value.ToString(),
                });
            }

            if (accountDto.HeadAccountId.HasValue)
            {
                account.HeadAccountId = accountDto.HeadAccountId.Value;
                changedFields.Add(new FieldChangeDto
                {
                    Field = "HeadAccountId",
                    OldValue = account.HeadAccountId.ToString() ?? string.Empty,
                    NewValue = accountDto.HeadAccountId.Value.ToString(),
                });
            }

            if (!string.IsNullOrWhiteSpace(accountDto.BillingEmail))
            {
                account.InvoiceEmailAddress = accountDto.BillingEmail;
                changedFields.Add(new FieldChangeDto
                {
                    Field = "InvoiceEmailAddress",
                    OldValue = account.InvoiceEmailAddress,
                    NewValue = accountDto.BillingEmail,
                });
            }

            if (!string.IsNullOrWhiteSpace(accountDto.BillingType))
            {
                account.InvoiceType = accountDto.BillingType;
                changedFields.Add(new FieldChangeDto
                {
                    Field = "InvoiceType",
                    OldValue = account.InvoiceType,
                    NewValue = accountDto.BillingType,
                });
            }

            if (!string.IsNullOrWhiteSpace(accountDto.NotificationEmailAddress))
            {
                account.NotificationEmailAddress = accountDto.NotificationEmailAddress;
                changedFields.Add(new FieldChangeDto
                {
                    Field = "NotificationEmailAddress",
                    OldValue = account.NotificationEmailAddress,
                    NewValue = accountDto.NotificationEmailAddress,
                });
            }

            account.UpdatedAt = DateTime.UtcNow;
            account.UpdatedBy = "system";
            account.Version += 1;

            return (new UpdateAccountResult
            {
                AccountId = account.AccountId,
                Version = account.Version,
                UpdatedAt = account.UpdatedAt ?? DateTime.UtcNow,
                UpdatedBy = 1,
            }, changedFields);
        }

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
        public async Task<string> GetInvoicingTypeAsync(Guid accountId)
        {
            var account = await context.Accounts.AsNoTracking().FirstOrDefaultAsync(acc => acc.AccountId == accountId);
            if (account == null)
            {
                throw new AccountNotFoundException(new ErrorResponses
                {
                    Code = "AccountNotFound",
                    Message = "Account not found.",
                    Details = new ErrorInfo
                    {
                        AccountId = accountId,
                    },
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            return account.InvoiceType ?? string.Empty;
        }

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
        public async Task<List<TimezoneDto>> GetAllTimezonesAsync()
        {
            return await context.Set<Timezone>()
                .AsNoTracking()
                .Select(t => new TimezoneDto
                {
                    Id = t.Id,
                    Name = t.Name,
                })
                .ToListAsync()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a timezone by its unique identifier.
        /// </summary>
        /// <param name="timezoneId">The unique identifier of the timezone.</param>
        /// <returns>
        /// A <see cref="TimezoneDto"/> containing the timezone ID and IANA name,
        /// or <c>null</c> if no matching timezone is found.
        /// </returns>
        public async Task<TimezoneDto?> GetTimezoneByIdAsync(int timezoneId)
        {
            var timezone = await context.Set<Timezone>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == timezoneId)
                .ConfigureAwait(false);
            if (timezone == null)
            {
                return null;
            }
            return new TimezoneDto
            {
                Id = timezone.Id,
                Name = timezone.Name,
            };
        }

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
        public async Task<(List<ProductAssociationDto> Products, int TotalCount)> GetAccountProductsAsync(
            Guid accountId,
            bool? isActive,
            int pageSize,
            int pageNumber)
        {
            bool accountExists = await context.Accounts
                .AsNoTracking()
                .AnyAsync(a => a.AccountId == accountId)
                .ConfigureAwait(false);

            if (!accountExists)
            {
                throw new AccountNotFoundException(new ErrorResponses
                {
                    Code = "AccountNotFound",
                    Message = "Account not found",
                    Details = new ErrorInfo
                    {
                        AccountId = accountId,
                    },
                    TimeStamp = DateTime.UtcNow.ToString(),
                });
            }

            var query = context.ProductAssociations
                .AsNoTracking()
                .Where(pa => pa.AccountId == accountId);

            if (isActive.HasValue)
            {
                query = query.Where(pa => pa.IsActive == isActive.Value);
            }

            int totalCount = await query.CountAsync().ConfigureAwait(false);
            int skip = (Math.Max(pageNumber, 1) - 1) * Math.Max(pageSize, 1);

            List<ProductAssociationDto> products = await query
                .OrderBy(pa => pa.ProductName)
                .Skip(skip)
                .Take(Math.Max(pageSize, 1))
                .Select(pa => new ProductAssociationDto
                {
                    ProductId = pa.ProductId,
                    ProductName = pa.ProductName,
                    IsActive = pa.IsActive,
                    LastSyncedAt = pa.LastSyncedAt,
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return (products, totalCount);
        }

        /// <summary>
        /// Asynchronously unlinks a sub-account from its head account.
        /// </summary>
        /// <param name="headAccountId">The unique identifier of the head account.</param>
        /// <param name="subAccountId">The unique identifier of the sub-account to be unlinked.</param>
        /// <returns>A task representing the asynchronous unlink operation.</returns>
        public async Task UnlinkSubAccountAsync(Guid headAccountId, Guid subAccountId)
        {
            var subAccountInfo = await context.Accounts.FirstOrDefaultAsync(account => account.AccountId == subAccountId);
            if (subAccountInfo != null)
            {
                subAccountInfo.HeadAccountId = null;
                subAccountInfo.Version += 1;
                subAccountInfo.UpdatedAt = DateTime.UtcNow;
            }

            var headAccountInfo = await context.Accounts.FirstOrDefaultAsync(account => account.AccountId == headAccountId);
            if (headAccountInfo != null)
            {
                // Check if there are any other sub-accounts linked to this head account
                bool hasOtherSubAccounts = await context.Accounts.AnyAsync(account => account.HeadAccountId == headAccountId && account.AccountId != subAccountId);
                if (!hasOtherSubAccounts)
                {
                    headAccountInfo.IsHeadAccount = false;
                    headAccountInfo.Version += 1;
                    headAccountInfo.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

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
        public async Task<bool> CheckTimezoneExistsAsync(string timezone)
        {
            return await context.Timezones.AnyAsync(a => a.Name == timezone).ConfigureAwait(false);
        }
    }
}
