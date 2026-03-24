// <copyright file="CreateAuditEntryCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Commands.CreateAuditEntryCommand
{
    using System;
    using AccountManager.Domain.Enums.AuditLog;
    using AccountManager.Domain.Events.Models;
    using MediatR;

    /// <summary>
    /// Command to create a new audit log entry.
    /// </summary>
    public class CreateAuditEntryCommand : IRequest<Guid>
    {
        /// <summary>
        /// Gets or sets the type of entity being audited.
        /// </summary>
        public AuditEntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the entity being audited.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of operation performed on the entity.
        /// </summary>
        public AuditOperation OperationType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who performed the operation.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the serialized state of the entity before the operation.
        /// </summary>
        public string? BeforeState { get; set; }

        /// <summary>
        /// Gets or sets the serialized state of the entity after the operation.
        /// </summary>
        public string? AfterState { get; set; }

        /// <summary>
        /// Gets or sets the fields that were changed during the operation.
        /// </summary>
        public string? ChangedFields { get; set; }

        /// <summary>
        /// Gets or sets the reason provided for the operation.
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Gets or sets the identifier of a related entity, if applicable.
        /// </summary>
        public Guid? RelatedEntityId { get; set; }

        /// <summary>
        /// Gets or sets the outcome of the operation. Defaults to <see cref="AuditOutcome.Success"/>.
        /// </summary>
        public AuditOutcome Outcome { get; set; } = AuditOutcome.Success;

        /// <summary>
        /// Gets or sets additional metadata associated with the audit entry.
        /// </summary>
        public EventMetadata? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier used to link related operations.
        /// </summary>
        public Guid? CorrelationId { get; set; }
    }
}
