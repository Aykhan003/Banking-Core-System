namespace Banking_Core_System.Interfaces;

public interface ITransferLimitService
{
    Task CreateTransferLimitForCustomerAsync(int customerId, decimal singleTransferLimit, decimal dailyTransferLimit);
    Task UpdateTransferLimitAsync(int customerId, decimal singleTransferLimit, decimal dailyTransferLimit);
    Task<bool> IsSingleTransactionLimitValidAsync(int customerId, decimal transactionAmount);
    Task<decimal> GetDailyTransferAmountAsync(int customerId);
    Task<decimal> GetRemainingDailyLimitAsync(int customerId);

}
