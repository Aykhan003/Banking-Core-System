using Banking_Core_System.Enums;
using Banking_Core_System.Models;

namespace Banking_Core_System.Interfaces;

public interface ICustomerService
{
    public Task<Customer> CreateCustomerAsync(string name, string surname, string email, string phoneNumber);
    public Task<Customer> UpdateCustomerAsync(int customerId, string name, string surname, string email, string phoneNumber);
    public Task DeleteCustomerAsync(int customerId);
    public Task<Customer> GetCustomerAsync(int customerId);
    public Task<List<Customer>> GetAllCustomersAsync();
    public Task DeactivateCustomerAsync(int customerId);
    public Task<List<BankAccount>> GetCustomerAllAccountsAsync(int customerId);
    public Task<List<Transaction>> GetCustomerRecentTransactionsAsync(int customerId, int count);
    public Task<Dictionary<Currency,decimal>> GetCustomerTotalBalanceByCurrencyAsync(int customerId);
    public Task<List<BankAccount>> GetAccountWithSuspiciousFailedTransactionsAsync(int failedTransactionThreshold);
}
