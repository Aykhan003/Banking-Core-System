using Microsoft.EntityFrameworkCore;
using Banking_Core_System.Models;

namespace Banking_Core_System.Configuration;


public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.ReferenceNumber).IsRequired();
        builder.Property(t => t.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(t => t.Fee).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(t => t.Currency).IsRequired();
        builder.Property(t => t.Type).IsRequired();
        builder.Property(t => t.Status).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.HasIndex(t => t.ReferenceNumber).IsUnique();
        builder.HasIndex(t => t.IdempotencyKey).IsUnique();
        builder.Property(t => t.IdempotencyKey).IsRequired();

        builder.HasOne(t => t.SourceAccount)
               .WithMany()
               .HasForeignKey(t => t.SourceAccountId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.DestinationAccount)
                .WithMany()
                .HasForeignKey(t => t.DestinationAccountId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.BankCard)
                .WithMany(bc => bc.Transactions)
                .HasForeignKey(t => t.BankCardId)
                .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.LedgerEntries)
               .WithOne()
               .HasForeignKey(le => le.TransactionId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}