using Banking_Core_System.Models;

namespace Banking_Core_System.Interfaces;

public interface ILedgerService
{
    Task<List<LedgerEntry>> GetLedgerEntriesAsync(int accountId);
    Task<List<LedgerEntry>> GetLedgerEntriesByDateRangeAsync(int accountId, DateTime startDate, DateTime endDate);
    Task<List<LedgerEntry>> GetLedgerEntriesByTransactionIdAsync(int transactionId);
    Task<decimal> CalculateBalanceFromLedgerAsync(int accountId);
    Task<bool> HasBalanceDiscrepancyAsync(int accountId);

}
