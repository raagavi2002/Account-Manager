// <copyright file="EventTypes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Events.Constants
{
    /// <summary>
    /// Defines event type constants used for publishing and consuming domain events
    /// within the Account Manager system.
    /// </summary>
    public static class EventTypes
    {
        // Published Events

        /// <summary>
        /// Event published when a new account is created.
        /// </summary>
        public const string AccountCreated = "ACCOUNT_CREATED";

        /// <summary>
        /// Event published when an existing account is updated.
        /// </summary>
        public const string AccountUpdated = "ACCOUNT_UPDATED";

        /// <summary>
        /// Event published when an account's status changes.
        /// </summary>
        public const string AccountStatusChanged = "ACCOUNT_STATUS_CHANGED";

        /// <summary>
        /// Event published when a new user is created.
        /// </summary>
        public const string UserCreated = "USER_CREATED";

        /// <summary>
        /// Event published when an existing user is updated.
        /// </summary>
        public const string UserUpdated = "USER_UPDATED";

        /// <summary>
        /// Event published when a user is deactivated.
        /// </summary>
        public const string UserDeactivated = "USER_DEACTIVATED";

        /// <summary>
        /// Event published when a user is activated.
        /// </summary>
        public const string UserActivated = "USER_ACTIVATED";

        /// <summary>
        /// Event published when a user profile is viewed/retrieved.
        /// </summary>
        public const string UserProfileViewed = "USER_PROFILE_VIEWED";

        /// <summary>
        /// Event published when an account is linked to another entity.
        /// </summary>
        public const string AccountLinked = "ACCOUNT_LINKED";

        /// <summary>
        /// Event published when an account is unlinked from another entity.
        /// </summary>
        public const string AccountUnlinked = "ACCOUNT_UNLINKED";

        /// <summary>
        /// Event published when product associations for an account are updated.
        /// </summary>
        public const string ProductAssociationUpdated = "PRODUCT_ASSOCIATION_UPDATED";

        // Consumed Events

        /// <summary>
        /// Event consumed when a new product is created.
        /// </summary>
        public const string ProductCreated = "PRODUCT_CREATED";

        /// <summary>
        /// Event consumed when an existing product is updated.
        /// </summary>
        public const string ProductUpdated = "PRODUCT_UPDATED";

        /// <summary>
        /// Event consumed when a product is deleted.
        /// </summary>
        public const string ProductDeleted = "PRODUCT_DELETED";

        /// <summary>
        /// Event consumed when a product is associated with an account.
        /// </summary>
        public const string ProductAssociated = "PRODUCT_ASSOCIATED";

        /// <summary>
        /// Event consumed when a product is disassociated from an account.
        /// </summary>
        public const string ProductDisassociated = "PRODUCT_DISASSOCIATED";

        // Error Events

        /// <summary>
        /// Event published when processing of an event fails.
        /// </summary>
        public const string EventProcessingFailed = "EVENT_PROCESSING_FAILED";

        // Audit Log Events

        /// <summary>
        /// Audit data published.
        /// </summary>
        public const string AuditEntryCreated = "AUDIT_LOG_CREATED";
    }
}
