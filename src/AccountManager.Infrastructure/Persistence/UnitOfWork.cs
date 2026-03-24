// <copyright file="UnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence;

using System.Data;
using AccountManager.Application.Abstractions;
using AccountManager.Application.Abstractions.Messaging;
using AccountManager.Domain.Interfaces;
using AccountManager.Infrastructure.Kafka.Configuration;
using AccountManager.Infrastructure.Outbox;
using AccountManager.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

/// <summary>
/// Implements the Unit of Work pattern to coordinate repository operations
/// and manage database transactions within a single business transaction.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AccountManagerDbContext dbContext;
    private readonly IOptions<KafkaOptions> kafkaOptions;

    // Lazy initialization to avoid circular dependencies
    private IAccountRepository? accountRepository;
    private IOutboxRepository? outboxRepository;
    private IUserRepository? userRepository;
    private IAccountRelationshipRepository accountRelationshipRepository;
    private IAuditLogRepository? auditLogRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="amDbContext">The Entity Framework database context.</param>
    /// <param name="kafkaoptions">The Kafka configuration options.</param>
    public UnitOfWork(
       AccountManagerDbContext amDbContext,
       IOptions<KafkaOptions> kafkaoptions)
    {
       dbContext = amDbContext;
       kafkaOptions = kafkaoptions;
    }

   /// <summary>
    /// Gets the account repository instance.
    /// Uses lazy initialization to create the repository on first access.
    /// </summary>
    public IAccountRepository Accounts =>
        accountRepository ??= new AccountRepository(dbContext);

    /// <summary>
    /// Gets the outbox repository instance.
    /// Uses lazy initialization to create the repository on first access.
    /// </summary>
    public IOutboxRepository Outbox =>
        outboxRepository ??= new OutboxRepository(dbContext, kafkaOptions);

    /// <summary>
    /// Gets the user repository instance.
    /// </summary>
    public IUserRepository User => userRepository ??= new UserRepository(dbContext);

    /// <summary>
    /// Gets the account relationship instance.
    /// </summary>
    public IAccountRelationshipRepository AccountRelationship => accountRelationshipRepository ??= new AccountRelationshipRepository(dbContext);

    /// <summary>
    /// Gets the audit log repository instance.
    /// Uses lazy initialization to create the repository on first access.
    /// </summary>
    public IAuditLogRepository AuditLogs =>
        auditLogRepository ??= new Repository.AuditLog.AuditLogRepository(dbContext);

    /// <summary>
    /// Persists all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return dbContext.SaveChangesAsync(true, cancellationToken);
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Begins a new database transaction with optional isolation level.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <param name="isolationLevel">The isolation level for the transaction. If null, uses the default isolation level.</param>
    /// <returns>A task that represents the asynchronous operation, containing the transaction wrapper.</returns>
    public async Task<ITransaction> BeginTransactionAsync(
        CancellationToken cancellationToken,
        IsolationLevel? isolationLevel = null)
    {
        IDbContextTransaction transaction = isolationLevel.HasValue
            ? await dbContext.Database.BeginTransactionAsync(isolationLevel.Value, cancellationToken)
            : await dbContext.Database.BeginTransactionAsync(cancellationToken);

        return new EfCoreTransaction(transaction);
    }

    /// <summary>
    /// Commits the specified transaction, persisting all changes to the database.
    /// </summary>
    /// <param name="transaction">The transaction to commit.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous commit operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the transaction is not of type <see cref="EfCoreTransaction"/>.</exception>
    public async Task CommitTransactionAsync(ITransaction transaction, CancellationToken cancellationToken)
    {
        if (transaction is not EfCoreTransaction efTransaction)
        {
            throw new InvalidOperationException("Invalid transaction object");
        }

        await efTransaction.Inner.CommitAsync(cancellationToken);
        await efTransaction.DisposeAsync();
    }

    /// <summary>
    /// Rolls back the specified transaction, discarding all changes.
    /// </summary>
    /// <param name="transaction">The transaction to roll back.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous rollback operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the transaction is not of type <see cref="EfCoreTransaction"/>.</exception>
    public async Task RollbackTransactionAsync(ITransaction transaction, CancellationToken cancellationToken)
    {
        if (transaction is not EfCoreTransaction efTransaction)
        {
            throw new InvalidOperationException("Invalid transaction object");
        }

        await efTransaction.Inner.RollbackAsync(cancellationToken);
        await efTransaction.DisposeAsync();
    }

    /// <summary>
    /// Executes the specified action within a database transaction using an execution strategy
    /// to handle transient failures. Automatically commits on success or rolls back on failure.
    /// </summary>
    /// <param name="action">The action to execute within the transaction.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is null.</exception>
    public async Task ExecuteInTransactionAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(action);
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await action(cancellationToken);
                await dbContext.SaveChangesAsync(true, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    /// <summary>
    /// Executes the specified function within a database transaction using an execution strategy
    /// to handle transient failures. Automatically commits on success or rolls back on failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="func">The function to execute within the transaction.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of the function.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="func"/> is null.</exception>
    public async Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> func,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(func);
        var strategy = dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await func(cancellationToken);
                await dbContext.SaveChangesAsync(true, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    /// <summary>
    /// Wraps an Entity Framework Core transaction to provide a common transaction abstraction.
    /// Ensures proper disposal of the underlying transaction resource.
    /// </summary>
    private sealed class EfCoreTransaction : ITransaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreTransaction"/> class.
        /// </summary>
        /// <param name="inner">The underlying EF Core database transaction.</param>
        public EfCoreTransaction(IDbContextTransaction inner)
        {
            Inner = inner;
        }

        /// <summary>
        /// Gets the underlying Entity Framework Core database transaction.
        /// </summary>
        public IDbContextTransaction Inner { get; }

        /// <summary>
        /// Gets a value indicating whether the transaction has been completed (committed or rolled back).
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Asynchronously disposes the transaction, ensuring it is properly cleaned up.
        /// </summary>
        /// <returns>A task representing the asynchronous dispose operation.</returns>
        public async ValueTask DisposeAsync()
        {
            if (!IsCompleted)
            {
                try
                {
                    await Inner.DisposeAsync();
                }
                finally
                {
                    IsCompleted = true;
                }
            }
        }
    }
}
