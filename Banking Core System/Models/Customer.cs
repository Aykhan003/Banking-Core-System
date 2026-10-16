namespace Banking_Core_System.Models;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string FinCode { get; set; } = null!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    public List<TransferLimit> TransferLimits { get; set; } = new List<TransferLimit>();
    override public string ToString()
    {
        return $"Customer(Id={Id}, FullName={FullName}, FinCode={FinCode}, Email={Email}, PhoneNumber={PhoneNumber}, CreatedAt={CreatedAt}, IsActive={IsActive})";
    }
}
