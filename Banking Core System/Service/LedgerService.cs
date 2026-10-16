using Banking_Core_System.Data;
using Banking_Core_System.Interfaces;
using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Service;

public class LedgerService : ILedgerService
{
    private readonly BankingCoreSystemDbContext _context;
    public LedgerService(BankingCoreSystemDbContext context)
    {
        _context = context;
    }
    public async Task<List<LedgerEntry>> GetLedgerEntriesAsync(int accountId)
    {
        if (!await _context.BankAccounts.AnyAsync(a => a.Id == accountId))
        {
            throw new InvalidOperationException("Bank account not found.");
        }
        return await _context.LedgerEntries
            .Where(e => e.BankAccountId == accountId)
            .ToListAsync();
    }
    public async Task<List<LedgerEntry>> GetLedgerEntriesByDateRangeAsync(int accountId, DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date cannot be later than end date.");
        }
        return await _context.LedgerEntries
            .Where(e => e.BankAccountId == accountId && e.CreatedAt >= startDate && e.CreatedAt <= endDate)
            .ToListAsync();
    }
    public async Task<List<LedgerEntry>> GetLedgerEntriesByTransactionIdAsync(int transactionId)
    {
        if (!await _context.Transactions.AnyAsync(t => t.Id == transactionId))
        {
            throw new InvalidOperationException("Transaction not found.");
        }
        if (!await _context.LedgerEntries.AnyAsync(e => e.TransactionId == transactionId))
        {
            throw new InvalidOperationException("No ledger entries found for the given transaction.");
        }
        return await _context.LedgerEntries
            .Where(e => e.TransactionId == transactionId)
            .ToListAsync();
    }
    public async Task<decimal> CalculateBalanceFromLedgerAsync(int accountId)
    {
        if (!await _context.BankAccounts.AnyAsync(a => a.Id == accountId))
        {
            throw new InvalidOperationException("Bank account not found.");
        }
        if (!await _context.LedgerEntries.AnyAsync(e => e.BankAccountId == accountId))
        {
            throw new InvalidOperationException("No ledger entries found for the given bank account.");
        }
        var ledgerEntries = await _context.LedgerEntries
            .Where(e => e.BankAccountId == accountId)
            .ToListAsync();
        return ledgerEntries.Sum(e => e.Amount);
    }
    public async Task<bool> HasBalanceDiscrepancyAsync(int accountId)
    {
        if (!await _context.BankAccounts.AnyAsync(a => a.Id == accountId))
        {
            throw new InvalidOperationException("Bank account not found.");
        }
        if (!await _context.LedgerEntries.AnyAsync(e => e.BankAccountId == accountId))
        {
            throw new InvalidOperationException("No ledger entries found for the given bank account.");
        }
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        var calculatedBalance = await CalculateBalanceFromLedgerAsync(accountId);
        return bankAccount.Balance != calculatedBalance;
    }
}
