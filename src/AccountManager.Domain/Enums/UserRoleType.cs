// <copyright file="UserRoleType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the roles that can be assigned to a user within the system.
    /// </summary>
    public enum UserRoleType
    {
        /// <summary>
        /// Represents a system administrator with full access privileges.
        /// </summary>
        [EnumMember(Value = "ADMIN")]
        Admin = 1,

        /// <summary>
        /// Represents a user responsible for managing client accounts.
        /// </summary>
        [EnumMember(Value = "ACCOUNTMANAGER")]
        AccountManager = 2,

        /// <summary>
        /// Represents a Customer Success Manager responsible for client satisfaction and support.
        /// </summary>
        [EnumMember(Value = "CSM")]
        CSM = 3,

        /// <summary>
        /// Represents the primary client contact with overall account visibility.
        /// </summary>
        [EnumMember(Value = "MAINCLIENT")]
        MainClient = 4,

        /// <summary>
        /// Represents a client user responsible for invoicing and billing-related activities.
        /// </summary>
        [EnumMember(Value = "INVOICINGCLIENT")]
        InvoicingClient = 5,

        /// <summary>
        /// Represents a client user responsible for operational or day-to-day activities.
        /// </summary>
        [EnumMember(Value = "OPERATIONCLIENT")]
        OperationsClient = 6,
    }
}
