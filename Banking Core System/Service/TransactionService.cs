using Banking_Core_System.Data;
using Banking_Core_System.Enums;
using Banking_Core_System.Interfaces;
using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Service;

public class TransactionService : ITransactionService
{
    private readonly BankingCoreSystemDbContext _context;
    public TransactionService(BankingCoreSystemDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> DepositAsync(int accountId, decimal amount, string idempotencyKey)
    {
        // Check for duplicate transaction using idempotency key
        var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey);
        if (existingTransaction != null)
        {
            return existingTransaction; // Return the existing transaction to ensure idempotency
        }
        if (amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be greater than zero.");
        }
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (bankAccount == null)
        {
            throw new InvalidOperationException("Bank account not found.");
        }
        bankAccount.Balance += amount;
        var transaction = new Transaction
        {
            ReferenceNumber = Guid.NewGuid().ToString(),
            SourceAccountId = null,
            DestinationAccountId = accountId,
            Amount = amount,
            Fee = 0,
            Currency = bankAccount.Currency,
            Status = TransactionStatus.Completed,
            Type = TransactionType.Deposit,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow,
            IdempotencyKey = idempotencyKey
        };
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> TransferAsync(int sourceAccountId, int destinationAccountId, decimal amount, string idempotencyKey)
    {
        // Check for duplicate transaction using idempotency key
        var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey);
        if (existingTransaction != null)
        {
            return existingTransaction; // Return the existing transaction to ensure idempotency
        }
        if (amount <= 0)
        {
            throw new ArgumentException("Transfer amount must be greater than zero.");
        }
        var sourceAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == sourceAccountId);
        var destinationAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == destinationAccountId);
        if (sourceAccount == null || destinationAccount == null)
        {
            throw new InvalidOperationException("Source or destination bank account not found.");
        }
        if (sourceAccountId == destinationAccountId)
        {
            throw new ArgumentException(
                "Source and destination accounts cannot be the same.");
        }
        if (sourceAccount.Currency != destinationAccount.Currency)
        {
            throw new InvalidOperationException(
                "Account currencies must match.");
        }
        if (sourceAccount.Balance < amount)
        {
            throw new InvalidOperationException("Insufficient funds in the source account.");
        }
        sourceAccount.Balance -= amount;
        destinationAccount.Balance += amount;
        var transaction = new Transaction
        {
            ReferenceNumber = Guid.NewGuid().ToString(),
            SourceAccountId = sourceAccountId,
            DestinationAccountId = destinationAccountId,
            Amount = amount,
            Fee = 0,
            Status = TransactionStatus.Completed,
            Type = TransactionType.Transfer,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow,
            IdempotencyKey = idempotencyKey
        };
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> WithdrawAsync(int accountId, decimal amount, string idempotencyKey)
    {
        var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey);
        if (existingTransaction != null)
        {
            return existingTransaction; // Return the existing transaction to ensure idempotency
        }
        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be greater than zero.");
        }
        var account = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account == null)
        {
            throw new InvalidOperationException("Bank account not found.");
        }
        if (account.Balance < amount)
        {
            throw new InvalidOperationException("Insufficient funds in the account.");
        }
        // Check for duplicate transaction using idempotency key
        account.Balance -= amount;
        var transaction = new Transaction
        {
            ReferenceNumber = Guid.NewGuid().ToString("N"),
            SourceAccountId = accountId,
            DestinationAccountId = null,
            Amount = amount,
            Fee = 0,
            Currency = account.Currency,
            Status = TransactionStatus.Completed,
            Type = TransactionType.Withdrawal,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow,
            IdempotencyKey = idempotencyKey
        };
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }
}
