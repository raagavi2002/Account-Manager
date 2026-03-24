// <copyright file="AuditLogRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Repository.AuditLog
{
    using System.Threading.Tasks;
    using AccountManager.Domain.Aggregates.AuditAggregate;
    using AccountManager.Domain.Interfaces;
    using AccountManager.Infrastructure.Persistence.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Implements persistence for audit log entries.
    /// </summary>
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AccountManagerDbContext dbContext;

        public AuditLogRepository(AccountManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            var entity = new AuditLogEntry
            {
                Id = auditLog.Id,
                EntityType = (int)auditLog.EntityType,
                EntityId = auditLog.EntityId,
                RelatedEntityId = auditLog.RelatedEntityId,
                Operation = (int)auditLog.OperationType,
                ActorUserId = auditLog.UserId,
                OccurredAtUtc = auditLog.OccurredAtUtc,
                Outcome = (int)auditLog.Outcome,
                Reason = auditLog.Reason,
                PreviousState = auditLog.BeforeState,
                NewState = auditLog.AfterState,
                ChangedFields = auditLog.ChangedFields,
                CorrelationId = auditLog.CorrelationId,
            };
            dbContext.AuditLogEntries.Add(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
