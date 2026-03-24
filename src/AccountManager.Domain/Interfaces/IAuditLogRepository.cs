// <copyright file="IAuditLogRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using AccountManager.Domain.Aggregates.AuditAggregate;

    /// <summary>
    /// Provides methods for persisting and retrieving audit log entries.
    /// </summary>
    public interface IAuditLogRepository
    {
        /// <summary>
        /// Persists a new audit log entry.
        /// </summary>
        /// <param name="auditLog">The audit log entry to persist.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(AuditLog auditLog);
    }
}
