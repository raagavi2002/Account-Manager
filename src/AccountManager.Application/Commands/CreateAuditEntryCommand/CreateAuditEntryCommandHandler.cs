// <copyright file="CreateAuditEntryCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.CreateAuditEntryCommand
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountManager.Domain.Aggregates.AuditAggregate;
    using AccountManager.Domain.Interfaces;
    using MediatR;

    /// <summary>
    /// Handles the creation of audit log entries.
    /// </summary>
    public class CreateAuditEntryCommandHandler : IRequestHandler<CreateAuditEntryCommand, Guid>
    {
        private readonly IAuditLogRepository auditLogRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAuditEntryCommandHandler"/> class.
        /// </summary>
        /// <param name="auditLogRepository">The repository used to persist audit log entries.</param>
        public CreateAuditEntryCommandHandler(IAuditLogRepository auditLogRepository)
        {
            this.auditLogRepository = auditLogRepository;
        }

        /// <summary>
        /// Handles the <see cref="CreateAuditEntryCommand"/> request by creating and persisting
        /// a new audit log entry.
        /// </summary>
        /// <param name="request">The command containing details of the audit entry to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with the unique identifier of the created audit log entry.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the audit log entry does not satisfy required invariants.
        /// </exception>
        public async Task<Guid> Handle(CreateAuditEntryCommand request, CancellationToken cancellationToken)
        {
            var auditLog = AuditLog.Create(
                request.EntityType,
                request.EntityId,
                request.OperationType,
                request.UserId,
                request.BeforeState,
                request.AfterState,
                request.ChangedFields,
                request.Outcome,
                request.Reason,
                request.RelatedEntityId,
                request.Metadata,
                request.CorrelationId);

            if (!auditLog.IsValid())
            {
                throw new InvalidOperationException("Invalid audit log entry. Invariants not satisfied.");
            }

            await auditLogRepository.AddAsync(auditLog);
            return auditLog.Id;
        }
    }
}
