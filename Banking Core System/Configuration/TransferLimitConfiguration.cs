using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Configuration;

public class TransferLimitConfiguration : IEntityTypeConfiguration<TransferLimit>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TransferLimit> builder)
    {
        builder.HasKey(tl => tl.Id);
        builder.Property(tl => tl.DailyLimit).IsRequired();
        builder.Property(tl => tl.SingleTransactionLimit).IsRequired();
        builder.Property(tl => tl.Currency).IsRequired();
        builder.HasOne(tl => tl.Customer)
            .WithMany(c => c.TransferLimits)
            .HasForeignKey(tl => tl.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
