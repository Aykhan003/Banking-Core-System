using Banking_Core_System.Data;
using Banking_Core_System.Enums;
using Banking_Core_System.Interfaces;
using Banking_Core_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking_Core_System.Service;

public class CustomerService : ICustomerService
{
    private readonly BankingCoreSystemDbContext _context;
    public CustomerService(BankingCoreSystemDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> CreateCustomerAsync(string fullName, string email, string phoneNumber)
    {
        bool emailExists = await _context.Customers.AnyAsync(c => c.Email == email);
        if (emailExists)
        {
            throw new InvalidOperationException("A customer with the same email already exists.");
        }
        var customer = new Customer
        {
            FullName = fullName,
            Email = email,
            PhoneNumber = phoneNumber,
            IsActive = true
        };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task DeactivateCustomerAsync(int customerId)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        if (customer == null)
        {
            throw new InvalidOperationException("Customer not found.");
        }
        customer.IsActive = false;
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        if (customer == null)
        {
            throw new InvalidOperationException("Customer not found.");
        }
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<List<BankAccount>> GetAccountWithSuspiciousFailedTransactionsAsync(int failedTransactionThreshold)
    {
        if (failedTransactionThreshold <= 0)
        {
            throw new ArgumentException("Failed transaction threshold must be greater than zero.");
        }
        return await _context.BankAccounts
        .Where(a => _context.Transactions.Count(t =>
            t.Status == TransactionStatus.Failed &&
            (t.SourceAccountId == a.Id ||
             t.DestinationAccountId == a.Id))
            > failedTransactionThreshold)
        .ToListAsync();
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        if (!await _context.Customers.AnyAsync())
        {
            throw new InvalidOperationException("No customers found.");
        }
        return await _context.Customers.ToListAsync();
    }

    public async Task<List<BankAccount>> GetCustomerAllAccountsAsync(int customerId)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        return await _context.BankAccounts.Where(a => a.CustomerId == customerId).ToListAsync();
    }

    public async Task<Customer> GetCustomerAsync(int customerId)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<List<Transaction>> GetCustomerRecentTransactionsAsync(int customerId, int count)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        if (count <= 0)
        {
            throw new ArgumentException("Count must be greater than zero.");
        }
        bool customerExists = await _context.BankAccounts.AnyAsync(a => a.CustomerId == customerId);
        if (!customerExists)
        {
            throw new InvalidOperationException("Customer has no accounts.");
        }
        return await _context.Transactions
            .Where(t => t.SourceAccount.CustomerId == customerId || t.DestinationAccount.CustomerId == customerId)
            .OrderByDescending(t => t.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<decimal> GetCustomerTotalBalanceAsync(int customerId)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        return await _context.BankAccounts.Where(a => a.CustomerId == customerId).SumAsync(a => a.Balance);
    }

    public async Task<Dictionary<Currency, decimal>> GetCustomerTotalBalanceByCurrencyAsync(int customerId)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == customerId))
        {
            throw new InvalidOperationException("Customer not found.");
        }
        return await _context.BankAccounts
            .Where(a => a.CustomerId == customerId)
            .GroupBy(a => a.Currency)
            .Select(g => new { Currency = g.Key, TotalBalance = g.Sum(a => a.Balance) })
            .ToDictionaryAsync(x => x.Currency, x => x.TotalBalance);
    }

    public async Task<Customer> UpdateCustomerAsync(int customerId, string fullName, string email, string phoneNumber)
    {
        var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            throw new InvalidOperationException("Customer not found.");
        }
        bool emailExists = await _context.Customers.AnyAsync(c => c.Email == email && c.Id != customerId);
        if (emailExists)
        {
            throw new InvalidOperationException("A customer with the same email already exists.");
        }
        customer.FullName = fullName;
        customer.Email = email;
        customer.PhoneNumber = phoneNumber;
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
}
