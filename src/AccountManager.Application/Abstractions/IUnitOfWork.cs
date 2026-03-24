// <copyright file="IUnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Abstractions
{
    using System.Data;
    using AccountManager.Application.Abstractions.Messaging;
    using AccountManager.Domain.Interfaces;

    /// <summary>
    /// Represents a unit of work that coordinates repositories and
    /// manages transactional boundaries.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the account repository.
        /// </summary>
        IAccountRepository Accounts { get; }

        /// <summary>
        /// Gets the outbox repository used for reliable message publishing.
        /// </summary>
        IOutboxRepository Outbox { get; }

        /// <summary>
        /// Gets the user repository.
        /// </summary>
        IUserRepository User { get; }

        /// <summary>
        /// Gets the account relationship repository.
        /// </summary>
        IAccountRelationshipRepository AccountRelationship { get; }

        /// <summary>
        /// Persists all changes made within the current unit of work.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <param name="isolationLevel">
        /// The optional transaction isolation level.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the started transaction.
        /// </returns>
        Task<ITransaction> BeginTransactionAsync(
            CancellationToken cancellationToken,
            IsolationLevel? isolationLevel = null);

        /// <summary>
        /// Commits the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction to commit.</param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <returns>A task that represents the asynchronous commit operation.</returns>
        Task CommitTransactionAsync(
            ITransaction transaction,
            CancellationToken cancellationToken);

        /// <summary>
        /// Rolls back the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction to roll back.</param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <returns>A task that represents the asynchronous rollback operation.</returns>
        Task RollbackTransactionAsync(
            ITransaction transaction,
            CancellationToken cancellationToken);

        /// <summary>
        /// Executes the specified action within a transaction.
        /// </summary>
        /// <param name="action">
        /// The action to execute within the transaction.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <returns>A task that represents the asynchronous execution.</returns>
        Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            CancellationToken cancellationToken);

        /// <summary>
        /// Executes the specified function within a transaction and returns a result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">
        /// The function to execute within the transaction.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to observe while waiting for the operation to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous execution.
        /// The task result contains the function result.
        /// </returns>
        Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken cancellationToken);
    }
}
