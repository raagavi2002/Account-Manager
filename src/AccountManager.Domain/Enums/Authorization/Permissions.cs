// <copyright file="Permissions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums.Authorization
{
    /// <summary>
    /// Centralized authorization permission keys organized by domain and action.
    /// </summary>
    public static class Permissions
    {
        /// <summary>
        /// Administrative permissions related to account and user management.
        /// </summary>
        public static class Administrative
        {
            /// <summary>
            /// View permissions for administrative data.
            /// </summary>
            public static class View
            {
                /// <summary>Permission to view the account name.</summary>
                public const string AccountName = "canViewAccountName";

                /// <summary>Permission to view account details.</summary>
                public const string Account = "canViewAccount";

                /// <summary>Permission to view users associated with an account.</summary>
                public const string Users = "canViewUsers";

                /// <summary>Permission to view a user's email address.</summary>
                public const string UserEmail = "canViewUserEmail";

                /// <summary>Permission to view timezone information.</summary>
                public const string Timezone = "canViewTimezone";

                /// <summary>Permission to view account address information.</summary>
                public const string Address = "canViewAddress";

                /// <summary>Permission to view the account status.</summary>
                public const string AccountStatus = "canViewAccountStatus";

                /// <summary>Permission to view products associated with an account.</summary>
                public const string Products = "canViewProducts";

                /// <summary>Permission to view orders associated with an account.</summary>
                public const string Orders = "canViewOrders";

                /// <summary>Permission to view the audit log.</summary>
                public const string AuditLog = "canViewAuditLog";
            }

            /// <summary>
            /// Update permissions for administrative data.
            /// </summary>
            public static class Update
            {
                /// <summary>Permission to update the account name.</summary>
                public const string AccountName = "canUpdateAccountName";

                /// <summary>Permission to update account details.</summary>
                public const string Account = "canUpdateAccount";

                /// <summary>Permission to update the account type.</summary>
                public const string AccountType = "canUpdateAccountType";

                /// <summary>Permission to update timezone information.</summary>
                public const string Timezone = "canUpdateTimezone";

                /// <summary>Permission to update account address information.</summary>
                public const string Address = "canUpdateAddress";

                /// <summary>Permission to update the account status.</summary>
                public const string AccountStatus = "canUpdateAccountStatus";

                /// <summary>Permission to update a user's email address.</summary>
                public const string UserEmail = "canUpdateUserEmail";
            }
        }

        /// <summary>
        /// Financial permissions related to billing and invoicing.
        /// </summary>
        public static class Financial
        {
            /// <summary>
            /// View permissions for financial data.
            /// </summary>
            public static class View
            {
                /// <summary>Permission to view currency information.</summary>
                public const string Currency = "canViewCurrency";

                /// <summary>Permission to view VAT number.</summary>
                public const string VatNumber = "canViewVatNumber";

                /// <summary>Permission to view billing email address.</summary>
                public const string BillingEmail = "canViewBillingEmail";

                /// <summary>Permission to view billing type.</summary>
                public const string BillingType = "canViewBillingType";
            }

            /// <summary>
            /// Update permissions for financial data.
            /// </summary>
            public static class Update
            {
                /// <summary>Permission to update currency information.</summary>
                public const string Currency = "canUpdateCurrency";

                /// <summary>Permission to update VAT number.</summary>
                public const string VatNumber = "canUpdateVatNumber";

                /// <summary>Permission to update billing email address.</summary>
                public const string BillingEmail = "canUpdateBillingEmail";

                /// <summary>Permission to update billing type.</summary>
                public const string BillingType = "canUpdateBillingType";

                /// <summary>Permission to update notification email address.</summary>
                public const string NotificationEmail = "canUpdateNotificationEmail";
            }
        }

        /// <summary>
        /// Gets all view permissions across all domains.
        /// </summary>
        public static IReadOnlySet<string> AllViewPermissions => new HashSet<string>
        {
            Administrative.View.AccountName,
            Administrative.View.Account,
            Administrative.View.Users,
            Administrative.View.UserEmail,
            Administrative.View.Timezone,
            Administrative.View.Address,
            Administrative.View.AccountStatus,
            Administrative.View.Products,
            Administrative.View.Orders,
            Administrative.View.AuditLog,
            Financial.View.Currency,
            Financial.View.VatNumber,
            Financial.View.BillingEmail,
            Financial.View.BillingType,
        };

        /// <summary>
        /// Gets all update permissions across all domains.
        /// </summary>
        public static IReadOnlySet<string> AllUpdatePermissions => new HashSet<string>
        {
            Administrative.Update.AccountName,
            Administrative.Update.Account,
            Administrative.Update.AccountType,
            Administrative.Update.Timezone,
            Administrative.Update.Address,
            Administrative.Update.AccountStatus,
            Administrative.Update.UserEmail,
            Financial.Update.Currency,
            Financial.Update.VatNumber,
            Financial.Update.BillingEmail,
            Financial.Update.BillingType,
            Financial.Update.NotificationEmail,
        };
    }
}
