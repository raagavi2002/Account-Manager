// <copyright file="BilllingType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the billing method used for an account.
    /// </summary>
    public enum BilllingType
    {
        /// <summary>
        /// Payment is made online at the time of purchase.
        /// </summary>
        [EnumMember(Value = "ONLINEPAYMENT")]
        OnlinePayment = 1,

        /// <summary>
        /// Charges are accumulated and invoiced on a monthly basis.
        /// </summary>
        [EnumMember(Value = "MONTHLYINVOICE")]
        MonthlyInvoice = 2,

        /// <summary>
        /// No invoice is generated and no billing is applied.
        /// </summary>
        [EnumMember(Value = "NOINVOICE")]
        NoInvoice = 3,
    }
}
