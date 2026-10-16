using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Configuration;

public class BankCardConfiguration : IEntityTypeConfiguration<BankCard>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BankCard> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.CardNumberMasked).IsRequired().HasMaxLength(20);
        builder.Property(b => b.ExpirationDate).IsRequired();
        builder.Property(b => b.Status).IsRequired();
        builder.Property(b => b.DailyWithdrawalLimit).IsRequired();

        builder.HasOne(b => b.BankAccount)
               .WithMany(ba => ba.BankCards)
               .HasForeignKey(b => b.BankAccountId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
