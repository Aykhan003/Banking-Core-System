using Banking_Core_System.Models;

namespace Banking_Core_System.Interfaces;

public interface ITransactionService
{
    Task<Transaction> DepositAsync(int accountId, decimal amount, string idempotencyKey);
    Task<Transaction> WithdrawAsync(int accountId, decimal amount, string idempotencyKey);
    Task<Transaction> TransferAsync(int sourceAccountId, int destinationAccountId, decimal amount, string idempotencyKey);
}
