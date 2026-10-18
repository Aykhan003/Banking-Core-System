using Banking_Core_System.Data;
using Banking_Core_System.Interfaces;
using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;
using Banking_Core_System.Enums;
using Banking_Core_System.Exceptions;

namespace Banking_Core_System.Service;

public class BankAccountService : IBankAccountService
{
    private readonly BankingCoreSystemDbContext _context;
    public BankAccountService(BankingCoreSystemDbContext context)
    {
        _context = context;
    }

    public async Task CloseAccountAsync(int accountId)
    {
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (bankAccount == null)
        {
            throw new AccountNotFoundException("Bank account not found.");
        }
        if (bankAccount.Status == Status.Closed)
        {
            throw new AccountClosedException("Bank account is already closed.");
        }
        bankAccount.Status = Status.Closed;
        await _context.SaveChangesAsync();
    }

    public async Task<BankAccount> CreateBankAccountAsync(BankAccount bankAccount)
    {
        if (bankAccount == null)
        {
            throw new ArgumentNullException(nameof(bankAccount), "Bank account cannot be null.");
        }
        if (!await _context.Customers.AnyAsync(c => c.Id == bankAccount.CustomerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        _context.BankAccounts.Add(bankAccount);
        await _context.SaveChangesAsync();
        return bankAccount;
    }

    public async Task DeleteBankAccountAsync(int accountId)
    {
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (bankAccount == null)
        {
            throw new AccountNotFoundException("Bank account not found.");
        }
        _context.BankAccounts.Remove(bankAccount);
        await _context.SaveChangesAsync();
    }

    public async Task FreezeAccountAsync(int accountId)
    {
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (bankAccount == null)
        {
            throw new AccountNotFoundException("Bank account not found.");
        }
        bankAccount.Status = Status.Frozen;
        await _context.SaveChangesAsync();
    }

    public async Task<decimal> GetAccountBalanceAsync(int accountId)
    {
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (bankAccount == null)
        {
            throw new AccountNotFoundException("Bank account not found.");
        }
        return bankAccount.Balance;
    }

    public async Task<BankAccount> GetAccountInformationAsync(int accountId)
    {
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (bankAccount == null)
        {
            throw new AccountNotFoundException("Bank account not found.");
        }
        return bankAccount;
    }

    public async Task<List<BankAccount>> GetAllBankAccountsAsync()
    {
        return await _context.BankAccounts.ToListAsync();
    }

    public async Task<BankAccount> GetBankAccountByIdAsync(int accountId)
    {
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (bankAccount == null)
        {
            throw new AccountNotFoundException("Bank account not found.");
        }
        return bankAccount;
    }

    public async Task UnfreezeAccountAsync(int accountId)
    {
        var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (bankAccount == null)
        {
            throw new AccountNotFoundException("Bank account not found.");
        }
        if (bankAccount.Status != Status.Frozen)
        {
            throw new AccountFrozenException("Bank account is not frozen.");
        }
        bankAccount.Status = Status.Active;
        await _context.SaveChangesAsync();
    }

    public async Task<BankAccount> UpdateBankAccountAsync(BankAccount bankAccount)
    {
        if (bankAccount == null)
        {
            throw new ArgumentNullException(nameof(bankAccount), "Bank account cannot be null.");
        }
        var existingAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == bankAccount.Id);
        if (existingAccount == null)
        {
            throw new AccountNotFoundException("Bank account not found.");
        }
        existingAccount.AccountNumber = bankAccount.AccountNumber;
        existingAccount.Balance = bankAccount.Balance;
        existingAccount.Status = bankAccount.Status;
        await _context.SaveChangesAsync();
        return existingAccount;
    }
}
