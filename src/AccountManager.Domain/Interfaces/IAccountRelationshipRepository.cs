// <copyright file="IAccountRelationshipRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Interfaces
{
    using AccountManager.Application.DTO;
    using AccountManager.Domain.DTO;
    using AccountManager.Domain.Enums;
    using AccountManager.Domain.Results;

    /// <summary>
    /// Defines persistence operations for managing relationships between head accounts and sub-accounts.
    /// </summary>
    public interface IAccountRelationshipRepository
    {
        /// <summary>
        /// Creates a relationship between a head account and a sub-account.
        /// </summary>
        /// <param name="headAccountId">The unique identifier of the head account.</param>
        /// <param name="subaccountId">The unique identifier of the sub-account to be linked.</param>
        /// <returns>
        /// A <see cref="LinkSubAccountResult"/> containing details of the created account relationship.
        /// </returns>
        Task<LinkSubAccountResult> CreateHeadSubAccountRelationshipAsync(Guid headAccountId, Guid subaccountId);

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
        Task<AccountRelationshipDto?> GetAccountRelationshipAsync(Guid headAccountId, Guid subaccountId);

        /// <summary>
        /// Deletes (marks inactive) the relationship between a head account and a subaccount.
        /// </summary>
        /// <param name="unlinkSubAccountDto">The information related to unlinking of head and sub account along with reason.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains <c>true</c> if the relationship was found and marked inactive;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if an unexpected error occurs during the database operation.
        /// </exception>
        Task<UnlinkSubAccountResult> UnlinkSubAccountAsync(UnlinkSubAccountDto unlinkSubAccountDto);
    }
}
