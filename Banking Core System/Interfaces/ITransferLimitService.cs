using Banking_Core_System.Enums;

namespace Banking_Core_System.Interfaces;

public interface ITransferLimitService
{
    Task CreateTransferLimitForCustomerAsync(int customerId, decimal singleTransferLimit, decimal dailyTransferLimit, Currency currency);
    Task UpdateTransferLimitAsync(int customerId, decimal singleTransferLimit, decimal dailyTransferLimit, Currency currency);
    Task<bool> IsSingleTransactionLimitValidAsync(int customerId, decimal transactionAmount);
    Task<decimal> GetDailyTransferAmountAsync(int customerId, Currency currency);
    Task<decimal> GetRemainingDailyLimitAsync(int customerId, Currency currency);

}
