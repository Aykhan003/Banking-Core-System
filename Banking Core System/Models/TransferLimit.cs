using Banking_Core_System.Enums;

namespace Banking_Core_System.Models;

public class TransferLimit
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal DailyLimit { get; set; }
    public decimal SingleTransactionLimit { get; set; }
    public Currency Currency { get; set; }

    override public string ToString()
    {
        return $"TransferLimit(Id={Id}, CustomerId={CustomerId}, DailyLimit={DailyLimit}, SingleTransactionLimit={SingleTransactionLimit}, Currency={Currency})";
    }
}
