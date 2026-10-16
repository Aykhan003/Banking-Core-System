using Banking_Core_System.Enums;

namespace Banking_Core_System.Models;

public class Transaction
{
    public int Id { get; set; }
    public string ReferenceNumber { get; set; } = null!;
    public int? SourceAccountId { get; set; }
    public BankAccount? SourceAccount { get; set; }
    public int? DestinationAccountId { get; set; }
    public BankAccount? DestinationAccount { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public Currency Currency { get; set; }
    public TransactionStatus Status { get; set; }
    public TransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public string IdempotencyKey { get; set; } = null!;
    public List<LedgerEntry> LedgerEntries { get; set; } = new List<LedgerEntry>();

    override public string ToString()
    {
        return $"Transaction(Id={Id}, ReferenceNumber={ReferenceNumber}, SourceAccountId={SourceAccountId}, DestinationAccountId={DestinationAccountId}, Amount={Amount}, Fee={Fee}, Currency={Currency}, Status={Status}, Type={Type}, CreatedAt={CreatedAt}, CompletedAt={CompletedAt}, IdempotencyKey={IdempotencyKey})";
    }
}
