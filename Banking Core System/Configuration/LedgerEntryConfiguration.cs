using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Configuration;

public class LedgerEntryConfiguration : IEntityTypeConfiguration<LedgerEntry>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<LedgerEntry> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Amount).IsRequired();
        builder.Property(e => e.BalanceAfter).IsRequired();
        builder.Property(e => e.EntryType).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(e => e.Transaction)
               .WithMany(t => t.LedgerEntries)
               .HasForeignKey(e => e.TransactionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.BankAccount)
                .WithMany(ba => ba.LedgerEntries)
                .HasForeignKey(e => e.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);
    }
    }
