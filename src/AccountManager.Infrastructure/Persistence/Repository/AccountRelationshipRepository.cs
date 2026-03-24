// <copyright file="AccountRelationshipRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Repository
{
    using AccountManager.Application.DTO;
    using AccountManager.Application.Utilities;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Domain.Results;
    using AccountManager.Infrastructure.Persistence.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Repository responsible for managing account relationship persistence operations.
    /// </summary>
    /// <param name="context">The database context used to access account relationship data.</param>
    public class AccountRelationshipRepository(AccountManagerDbContext context) : IAccountRelationshipRepository
    {
        /// <summary>
        /// Creates a relationship between a head account and a sub-account.
        /// </summary>
        /// <param name="headAccountId">The unique identifier of the head account.</param>
        /// <param name="subaccountId">The unique identifier of the sub-account to be linked.</param>
        /// <returns>
        /// A <see cref="LinkSubAccountResult"/> containing details of the newly created relationship.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when an error occurs while creating the account relationship.
        /// </exception>
        public async Task<LinkSubAccountResult> CreateHeadSubAccountRelationshipAsync(Guid headAccountId, Guid subaccountId)
        {
            try
            {
                AccountRelationship accountRelationship = new AccountRelationship
                {
                    HeadAccountId = headAccountId,
                    SubAccountId = subaccountId,
                    EstablishedBy = 1,
                    EstablishedAt = DateTime.UtcNow,
                    RelationshipStatus = EnumParser.GetEnumMemberValue<AccountRelationshipStatus>(AccountRelationshipStatus.Active),
                    Version = 1,
                };

                await context.AccountRelationships.AddAsync(accountRelationship);

                return new LinkSubAccountResult
                {
                    RelationshipId = accountRelationship.AccountRelationshipId,
                    HeadAccountId = headAccountId,
                    SubAccountId = subaccountId,
                    LinkedAt = accountRelationship.EstablishedAt,
                    LinkedBy = accountRelationship.EstablishedBy.ToString(),
                };
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes (marks inactive) the relationship between a head account and a subaccount.
        /// </summary>
        /// <param name="unlinkSubAccountDto">The information related to unlinking of head and sub account along with reason.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains <c>true</c> if the relationship was found and marked inactive;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method does not physically delete the relationship record. 
        /// Instead, it updates the status to <see cref="AccountRelationshipStatus.Inactive"/>
        /// and sets metadata such as <c>RemovedAt</c> and <c>RemovedBy</c>.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if an unexpected error occurs during the database operation.
        public async Task<UnlinkSubAccountResult> UnlinkSubAccountAsync(UnlinkSubAccountDto unlinkSubAccountDto)
        {
            try
            {
                var validRelationStatus = EnumParser.GetEnumMemberValue<AccountRelationshipStatus>(AccountRelationshipStatus.Active);
                var relationship = await context.AccountRelationships.FirstOrDefaultAsync(ar => ar.HeadAccountId == unlinkSubAccountDto.HeadAccountId && ar.SubAccountId == unlinkSubAccountDto.SubAccountId && ar.RelationshipStatus == validRelationStatus);

                if (relationship == null)
                {
                    return new UnlinkSubAccountResult();
                }

                relationship.RelationshipStatus = EnumParser.GetEnumMemberValue<AccountRelationshipStatus>(AccountRelationshipStatus.Inactive);
                relationship.RemovedAt = DateTime.UtcNow;
                relationship.RemovedBy = 1;
                return new UnlinkSubAccountResult
                {
                    SubAccountId = unlinkSubAccountDto.SubAccountId,
                    FormerHeadAccountId = unlinkSubAccountDto.HeadAccountId,
                    UnlinkedAt = relationship.RemovedAt.Value,
                    UnlinkedBy = Guid.NewGuid(), // Assuming we would replace this with the actual user performing the unlinking action
                    Reason = unlinkSubAccountDto.Reason,
                };
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves an active account relationship between a head account and a subaccount.
        /// </summary>
        /// <param name="headAccountId">
        /// The unique identifier of the head (parent) account.
        /// </param>
        /// <param name="subaccountId">
        /// The unique identifier of the sub (child) account.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains an <see cref="AccountRelationshipDto"/> if an active relationship exists;
        /// otherwise, <c>null</c>.
        /// </returns>
        /// <remarks>
        /// This method only returns relationships with a status of <see cref="AccountRelationshipStatus.Active"/>.
        /// If no active relationship is found, the result will be <c>null</c>.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if an unexpected error occurs during the database query.
        /// </exception>
        public async Task<AccountRelationshipDto?> GetAccountRelationshipAsync(Guid headAccountId, Guid subaccountId)
        {
            try
            {
                var validRelationStatus = EnumParser.GetEnumMemberValue<AccountRelationshipStatus>(AccountRelationshipStatus.Active);
                var relationship = await context.AccountRelationships.AsNoTracking()
                    .FirstOrDefaultAsync(ar => ar.HeadAccountId == headAccountId
                                            && ar.SubAccountId == subaccountId
                                            && ar.RelationshipStatus == validRelationStatus);

                if (relationship == null)
                {
                    return null;
                }

                return new AccountRelationshipDto
                {
                    AccountRelationshipId = relationship.AccountRelationshipId,
                    EstablishedBy = relationship.EstablishedBy,
                    EstablishedAt = relationship.EstablishedAt,
                    RelationshipStatus = relationship.RelationshipStatus,
                    Version = relationship.Version,
                    HeadAccountId = relationship.HeadAccountId ?? Guid.Empty,
                    SubAccountId = relationship.SubAccountId ?? Guid.Empty,
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
