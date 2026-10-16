using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Configuration;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.AccountNumber).IsUnique();
        builder.Property(b => b.AccountNumber).IsRequired();
        builder.Property(b => b.Balance).IsRequired();
        builder.Property(b => b.Currency).IsRequired();
        builder.Property(b => b.Status).IsRequired();
        builder.Property(b => b.CreatedAt).IsRequired();
        
        builder.HasMany(b => b.BankCards)
               .WithOne(bc => bc.BankAccount)
               .HasForeignKey(bc => bc.BankAccountId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Customers)
               .WithMany(c => c.BankAccounts)
               .HasForeignKey(b => b.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
