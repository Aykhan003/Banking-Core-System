using Banking_Core_System.Models;

namespace Banking_Core_System.Interfaces;

public interface IBankAccountService
{
    Task<BankAccount> CreateBankAccountAsync(BankAccount bankAccount);
    Task<BankAccount> GetBankAccountByIdAsync(int accountId);
    Task<List<BankAccount>> GetAllBankAccountsAsync();
    Task<BankAccount> UpdateBankAccountAsync(BankAccount bankAccount);
    Task DeleteBankAccountAsync(int accountId);
    Task<decimal> GetAccountBalanceAsync(int accountId);
    Task CloseAccountAsync(int accountId);
    Task FreezeAccountAsync(int accountId);
    Task UnfreezeAccountAsync(int accountId);
    Task<BankAccount> GetAccountInformationAsync(int accountId);
}
