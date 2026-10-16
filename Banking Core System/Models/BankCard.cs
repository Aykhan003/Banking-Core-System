using Banking_Core_System.Enums;

namespace Banking_Core_System.Models;

public class BankCard
{
    public int Id { get; set; }
    public int BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; } = null!;
    public string CardNumberMasked { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }
    public Status Status { get; set; }
    public decimal DailyWithdrawalLimit { get; set; }
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    override public string ToString()
    {
        return $"BankCard(Id={Id}, BankAccountId={BankAccountId}, CardNumberMasked={CardNumberMasked}, ExpirationDate={ExpirationDate}, Status={Status}, DailyWithdrawalLimit={DailyWithdrawalLimit})";
    }
}
