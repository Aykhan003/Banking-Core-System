using Banking_Core_System.Enums;

namespace Banking_Core_System.Models;

public class LedgerEntry
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;
    public int BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; } = null!;
    public EntryType EntryType { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    override public string ToString()
    {
        return $"LedgerEntry(Id={Id}, TransactionId={TransactionId}, BankAccountId={BankAccountId}, EntryType={EntryType}, Amount={Amount}, BalanceAfter={BalanceAfter}, CreatedAt={CreatedAt})";
    }
}
