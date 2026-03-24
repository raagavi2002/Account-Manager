using System;
using System.Collections.Generic;
using AccountManager.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Infrastructure.Persistence;

public partial class AccountManagerDbContext : DbContext
{
    public AccountManagerDbContext(DbContextOptions<AccountManagerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRelationship> AccountRelationships { get; set; }

    public virtual DbSet<AuditLogEntry> AuditLogEntries { get; set; }

    public virtual DbSet<KafkaProducedEvent> KafkaProducedEvents { get; set; }

    public virtual DbSet<ProductAssociation> ProductAssociations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Timezone> Timezones { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("accounts_pkey");

            entity.HasIndex(e => e.AccountName, "idx_accounts_name")
                .IsUnique()
                .HasFilter("(deleted_at IS NULL)");

            entity.HasIndex(e => e.AccountStatus, "idx_accounts_status").HasFilter("(deleted_at IS NULL)");

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.AccountStatus).HasDefaultValueSql("'INACTIVE'::character varying");
            entity.Property(e => e.AccountType).HasDefaultValueSql("'PROFESSIONAL'::character varying");
            entity.Property(e => e.Currency).HasDefaultValueSql("'USD'::character varying");
            entity.Property(e => e.DateFormat).HasDefaultValueSql("'MM/DD/YYYY'::character varying");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsHeadAccount).HasDefaultValue(false);
            entity.Property(e => e.Locale).HasDefaultValueSql("'en-US'::character varying");
            entity.Property(e => e.TimeFormat).HasDefaultValueSql("'12h'::character varying");
            entity.Property(e => e.Timezone).HasDefaultValueSql("'America/New_York'::character varying");
            entity.Property(e => e.Version).HasDefaultValue(1);
        });

        modelBuilder.Entity<AccountRelationship>(entity =>
        {
            entity.HasKey(e => e.AccountRelationshipId).HasName("account_relationships_pkey");

            entity.Property(e => e.AccountRelationshipId).UseIdentityAlwaysColumn();
            entity.Property(e => e.EstablishedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.RelationshipStatus).HasDefaultValueSql("'ACTIVE'::character varying");
            entity.Property(e => e.Version).HasDefaultValue(1);
        });

        modelBuilder.Entity<AuditLogEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_log_entries_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.IsGdprRequest).HasDefaultValue(false);
            entity.Property(e => e.OccurredAtUtc).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<KafkaProducedEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kafka_produced_events_pkey");

            entity.Property(e => e.ProducedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Status).HasDefaultValueSql("'PENDING'::character varying");
        });

        modelBuilder.Entity<ProductAssociation>(entity =>
        {
            entity.HasKey(e => e.ProductAssociationId).HasName("product_associations_pkey");

            entity.HasIndex(e => e.IsActive, "idx_product_active").HasFilter("(is_active = true)");

            entity.Property(e => e.ProductAssociationId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastSyncedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ProductStatus).HasDefaultValueSql("'ACTIVE'::character varying");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Version).HasDefaultValue(1);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Timezone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("timezone_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LoginCount).HasDefaultValue(0);
            entity.Property(e => e.Version).HasDefaultValue(1);

            entity.HasOne(d => d.Account).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accounts");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("user_roles_pkey");

            entity.Property(e => e.UserRoleId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_roles_role_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasConstraintName("user_roles_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
