using Banking_Core_System.Configuration;
using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Data;

public class BankingCoreSystemDbContext : DbContext
{
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=BankingCoreSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
    }

    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<BankAccount> BankAccounts { get; set; } = null!;
    public DbSet<BankCard> BankCards { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<LedgerEntry> LedgerEntries { get; set; } = null!;
    public DbSet<TransferLimit> TransferLimits { get; set; } = null!;
}
