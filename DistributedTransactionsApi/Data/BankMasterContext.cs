using System;
using System.Collections.Generic;
using DistributedTransactionsApi.Data.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace DistributedTransactionsApi.Data;

public partial class BankMasterContext : DbContext
{
    public BankMasterContext(DbContextOptions<BankMasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<MasterUser> MasterUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A630750956");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNumber)
                .IsRequired()
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("money");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccountType).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AccountTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_AccountType");
        });

        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.AccountTypeId).HasName("PK__AccountT__8F9585AF2880F82E");

            entity.ToTable("AccountType");

            entity.Property(e => e.AccountTypeId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BED179CC743");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MasterUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C0EB0D719");

            entity.ToTable("MasterUser");

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Department).WithMany(p => p.MasterUsers)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Department");

            entity.HasMany(d => d.Accounts).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserAccount",
                    r => r.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserAccount_Account"),
                    l => l.HasOne<MasterUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserAccount_User"),
                    j =>
                    {
                        j.HasKey("UserId", "AccountId").HasName("PK__UserAcco__64C11616A5B53EFB");
                        j.ToTable("UserAccount");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
