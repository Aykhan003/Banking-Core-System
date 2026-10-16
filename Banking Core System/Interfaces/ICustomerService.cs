using Banking_Core_System.Enums;
using Banking_Core_System.Models;

namespace Banking_Core_System.Interfaces;

public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(string fullName,string finCode, string email, string phoneNumber);
    Task<Customer> UpdateCustomerAsync(int customerId, string fullName, string email, string phoneNumber);
    Task DeleteCustomerAsync(int customerId);
    Task<Customer> GetCustomerAsync(int customerId);
    Task<List<Customer>> GetAllCustomersAsync();
    Task DeactivateCustomerAsync(int customerId);
    Task<List<BankAccount>> GetCustomerAllAccountsAsync(int customerId);
    Task<List<Transaction>> GetCustomerRecentTransactionsAsync(int customerId, int count);
    Task<Dictionary<Currency,decimal>> GetCustomerTotalBalanceByCurrencyAsync(int customerId);
    Task<decimal> GetCustomerTotalBalanceAsync(int customerId);
    Task<List<BankAccount>> GetAccountWithSuspiciousFailedTransactionsAsync(int failedTransactionThreshold);
}
