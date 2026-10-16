using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.FullName).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.FinCode).IsUnique();
        builder.Property(c => c.FinCode).IsRequired().HasMaxLength(20);
        builder.HasIndex(c => c.Email).IsUnique();
        builder.Property(c => c.Email).HasMaxLength(100);
        builder.HasIndex(c => c.PhoneNumber).IsUnique();
        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(15);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.IsActive).IsRequired();

        builder.HasMany(c => c.BankAccounts)
               .WithOne(ba => ba.Customers)
               .HasForeignKey(ba => ba.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.TransferLimits)
               .WithOne(tl => tl.Customer)
               .HasForeignKey(tl => tl.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
