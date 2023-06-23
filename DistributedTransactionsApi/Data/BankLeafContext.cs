using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DistributedTransactionsApi.Data.Models;

namespace DistributedTransactionsApi.Data;

public partial class BankLeafContext : DbContext
{
    public BankLeafContext()
    {
    }

    public BankLeafContext(DbContextOptions<BankLeafContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A6400E430E");

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
            entity.HasKey(e => e.AccountTypeId).HasName("PK__AccountT__8F9585AF8A08AB02");

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

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6BF9F25B45");

            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CAECE03A9");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
