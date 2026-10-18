using Banking_Core_System.Data;
using Banking_Core_System.Interfaces;
using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;
using Banking_Core_System.Enums;
namespace Banking_Core_System.Service;

public class TransferLimitService : ITransferLimitService
{
    private readonly BankingCoreSystemDbContext _context;
    public TransferLimitService(BankingCoreSystemDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsSingleTransactionLimitValidAsync(int customerId, decimal transactionAmount)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        var transferLimit = await _context.TransferLimits.FirstOrDefaultAsync(tl => tl.CustomerId == customerId);
        if (transferLimit == null)
        {
            throw new InvalidOperationException("Transfer limit for this customer not found.");
        }
        return transactionAmount > 0 && transactionAmount <= transferLimit.SingleTransactionLimit;
    }

    public async Task CreateTransferLimitForCustomerAsync(int customerId, decimal singleTransferLimit, decimal dailyTransferLimit, Currency currency)
    {
        if (singleTransferLimit <= 0 || dailyTransferLimit <= 0)
        {
            throw new ArgumentException("Transfer limits must be greater than zero.");
        }
        if (singleTransferLimit > dailyTransferLimit)
        {
            throw new ArgumentException("Single transfer limit cannot exceed daily transfer limit.");
        }
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        if (await _context.TransferLimits.AnyAsync(tl => tl.CustomerId == customerId && tl.Currency == currency))
        {
            throw new InvalidOperationException($"Transfer limit for this customer in {currency} already exists.");
        }
        var transferLimit = new TransferLimit
        {
            CustomerId = customerId,
            SingleTransactionLimit = singleTransferLimit,
            DailyLimit = dailyTransferLimit,
            Currency = currency
        };
        _context.TransferLimits.Add(transferLimit);
        await _context.SaveChangesAsync();
    }

    public async Task<decimal> GetDailyTransferAmountAsync(int customerId,Currency currency)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        return await _context.LedgerEntries
            .Where(le =>
                le.BankAccount.CustomerId == customerId &&
                le.BankAccount.Currency == currency &&
                le.EntryType == EntryType.Debit &&
                le.CreatedAt >= today &&
                le.CreatedAt < tomorrow)
            .SumAsync(le => le.Amount);
    }

    public async Task<decimal> GetRemainingDailyLimitAsync(int customerId, Currency currency)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        var transferLimit = await _context.TransferLimits.FirstOrDefaultAsync(tl => tl.CustomerId == customerId);
        if (transferLimit == null)
        {
            throw new InvalidOperationException("Transfer limit for this customer not found.");
        }
        var today = DateTime.UtcNow.Date;
        var dailyTransferAmount = await GetDailyTransferAmountAsync(customerId, currency);
        return Math.Max(0,transferLimit.DailyLimit - dailyTransferAmount);
    }

    public async Task UpdateTransferLimitAsync(int customerId, decimal singleTransferLimit, decimal dailyTransferLimit, Currency currency)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }

        var transferLimit = await _context.TransferLimits
            .FirstOrDefaultAsync(tl =>
                tl.CustomerId == customerId &&
                tl.Currency == currency);

        if (transferLimit == null)
        {
            throw new InvalidOperationException(
                "Transfer limit for this customer and currency not found.");
        }

        transferLimit.SingleTransactionLimit = singleTransferLimit;
        transferLimit.DailyLimit = dailyTransferLimit;

        await _context.SaveChangesAsync();
    }
}
