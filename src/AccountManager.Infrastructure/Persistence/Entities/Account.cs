// <copyright file="Account.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents an account entity in the Account Manager system.
/// </summary>
[Table("accounts", Schema = "am")]
public partial class Account
{
    /// <summary>
    /// Gets or sets the name of the account.
    /// </summary>
    [Column("account_name")]
    [StringLength(255)]
    public string AccountName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of the account (e.g., PROFESSIONAL, ENTERPRISE).
    /// </summary>
    [Column("account_type")]
    [StringLength(50)]
    public string AccountType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the current status of the account (e.g., ACTIVE, INACTIVE).
    /// </summary>
    [Column("account_status")]
    [StringLength(50)]
    public string AccountStatus { get; set; } = null!;

    /// <summary>
    /// Gets or sets the street address of the account.
    /// </summary>
    [Column("address_street")]
    [StringLength(200)]
    public string AddressStreet { get; set; } = null!;

    /// <summary>
    /// Gets or sets the secondary street address line (optional).
    /// </summary>
    [Column("address_street2")]
    [StringLength(200)]
    public string? AddressStreet2 { get; set; }

    /// <summary>
    /// Gets or sets the city of the account's address.
    /// </summary>
    [Column("address_city")]
    [StringLength(100)]
    public string AddressCity { get; set; } = null!;

    /// <summary>
    /// Gets or sets the state or province of the account's address.
    /// </summary>
    [Column("address_state")]
    [StringLength(50)]
    public string AddressState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the postal code of the account's address.
    /// </summary>
    [Column("address_postal_code")]
    [StringLength(20)]
    public string AddressPostalCode { get; set; } = null!;

    /// <summary>
    /// Gets or sets the country of the account's address.
    /// </summary>
    [Column("address_country", TypeName = "character varying")]
    public string AddressCountry { get; set; } = null!;

    /// <summary>
    /// Gets or sets the currency code used by the account (e.g., USD, EUR).
    /// </summary>
    [Column("currency")]
    [StringLength(3)]
    public string Currency { get; set; } = null!;

    /// <summary>
    /// Gets or sets the timezone for the account (e.g., America/New_York).
    /// </summary>
    [Column("timezone", TypeName = "character varying")]
    public string Timezone { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date format preference for the account (e.g., MM/DD/YYYY).
    /// </summary>
    [Column("date_format")]
    [StringLength(20)]
    public string DateFormat { get; set; } = null!;

    /// <summary>
    /// Gets or sets the time format preference for the account (e.g., 12h, 24h).
    /// </summary>
    [Column("time_format")]
    [StringLength(10)]
    public string TimeFormat { get; set; } = null!;

    /// <summary>
    /// Gets or sets the locale for the account (e.g., en-US, fr-FR).
    /// </summary>
    [Column("locale")]
    [StringLength(10)]
    public string Locale { get; set; } = null!;

    /// <summary>
    /// Gets or sets the VAT (Value Added Tax) number for the account (optional).
    /// </summary>
    [Column("vat_number")]
    [StringLength(50)]
    public string? VatNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this account is a head account in a hierarchical structure.
    /// </summary>
    [Column("is_head_account")]
    public bool IsHeadAccount { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the account manager responsible for this account (optional).
    /// </summary>
    [Column("account_manager_id")]
    public Guid? AccountManagerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the Customer Success Manager (CSM) for this account (optional).
    /// </summary>
    [Column("csm_id")]
    public Guid? CsmId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was created.
    /// </summary>
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the account.
    /// </summary>
    [Column("created_by")]
    [StringLength(255)]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was last updated.
    /// </summary>
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last updated the account.
    /// </summary>
    [Column("updated_by")]
    [StringLength(255)]
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was activated.
    /// </summary>
    [Column("activated_at")]
    public DateTime? ActivatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was deactivated.
    /// </summary>
    [Column("deactivated_at")]
    public DateTime? DeactivatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the account was soft-deleted.
    /// </summary>
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Gets or sets the version number for optimistic concurrency control.
    /// </summary>
    [Column("version")]
    public int Version { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the account is currently active.
    /// </summary>
    [Column("is_active")]
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the email address for receiving invoices (optional).
    /// </summary>
    [Column("invoice_email_address", TypeName = "character varying")]
    public string? InvoiceEmailAddress { get; set; }

    /// <summary>
    /// Gets or sets the type of invoice delivery (optional).
    /// </summary>
    [Column("invoice_type", TypeName = "character varying")]
    public string? InvoiceType { get; set; }

    /// <summary>
    /// Gets or sets the email address for receiving notifications (optional).
    /// </summary>
    [Column("notification_email_address", TypeName = "character varying")]
    public string? NotificationEmailAddress { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the account.
    /// </summary>
    [Key]
    [Column("account_id")]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the parent head account, if this is a child account (optional).
    /// </summary>
    [Column("head_account_id")]
    public Guid? HeadAccountId { get; set; }

    /// <summary>
    /// Gets or sets the collection of users associated with this account.
    /// </summary>
    [InverseProperty("Account")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
