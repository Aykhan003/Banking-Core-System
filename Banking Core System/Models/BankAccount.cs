using Banking_Core_System.Enums;

namespace Banking_Core_System.Models;

public class BankAccount
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customers { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public Currency Currency { get; set; }
    public decimal Balance { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int RowVersion { get; set; }
    public List<BankCard> BankCards { get; set; } = new List<BankCard>();
    public List<LedgerEntry> LedgerEntries { get; set; } = new List<LedgerEntry>();
    override public string ToString()
    {
        return $"BankAccount(Id={Id}, CustomerId={CustomerId}, AccountNumber={AccountNumber}, Currency={Currency}, Balance={Balance}, Status={Status}, CreatedAt={CreatedAt})";
    }
}
