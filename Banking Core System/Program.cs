using Banking_Core_System.Data;
using Banking_Core_System.Enums;
using Banking_Core_System.Models;
using Banking_Core_System.Service;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Banking_Core_System;

public class Program
{
    public static async Task Main(string[] args)
    {
        var context = new BankingCoreSystemDbContext();
        var customerService = new CustomerService(context);
        var bankAccountService = new BankAccountService(context);
        var transactionService = new TransactionService(context);
        var ledgerService = new LedgerService(context);
        var transferLimitService = new TransferLimitService(context);

        while (true)
        {
            Console.Clear();

            Console.WriteLine("===== BANKING CORE SYSTEM =====");
            Console.WriteLine("1. Create Customer");
            Console.WriteLine("2. Create Bank Account");
            Console.WriteLine("3. Show Account Balance");
            Console.WriteLine("4. Deposit Money");
            Console.WriteLine("5. Withdraw Money");
            Console.WriteLine("6. Transfer Between Accounts");
            Console.WriteLine("7. Show Account History");
            Console.WriteLine("8. Freeze Account");
            Console.WriteLine("9. Activate Account");
            Console.WriteLine("10. Show Transfer Limits");
            Console.WriteLine("11. Financial Reports");
            Console.WriteLine("12. Ledger Audit");
            Console.WriteLine("0. Exit");
            Console.WriteLine("================================");

            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        // Create Customer
                        Console.Write("Enter Full Name: ");
                        string? fullName = Console.ReadLine();
                        Console.Write("Enter FIN Code: ");
                        string? finCode = Console.ReadLine();
                        Console.Write("Enter Email: ");
                        string? email = Console.ReadLine();
                        Console.Write("Enter Phone Number: ");
                        string? phoneNumber = Console.ReadLine();

                        if (!string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(finCode) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(phoneNumber))
                        {
                            var customer = await customerService.CreateCustomerAsync(fullName, finCode, email, phoneNumber);
                            Console.WriteLine($"Customer created successfully! Customer ID: {customer.Id}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please provide all required information.");
                        }
                        break;

                    case "2":
                        // Create Bank Account
                        int customerId = ReadId("Enter Customer ID: ");
                        Console.Write("Enter Account Number: ");
                        string? accountNumber = Console.ReadLine();
                        Console.Write("Enter Initial Balance: ");
                        decimal initialBalance = ReadAmount("Please enter a valid amount: ");
                        Console.WriteLine("Select Currency (1-AZN, 2-USD, 3-TRY): ");
                        string? currencyChoice = Console.ReadLine();

                        Currency currency = currencyChoice switch
                        {
                            "1" => Currency.AZN,
                            "2" => Currency.USD,
                            "3" => Currency.TRY,
                            _ => Currency.AZN
                        };

                        if (!string.IsNullOrEmpty(accountNumber))
                        {
                            var bankAccount = new BankAccount
                            {
                                CustomerId = customerId,
                                AccountNumber = accountNumber,
                                Balance = initialBalance,
                                Currency = currency,
                                Status = Status.Active,
                                CreatedAt = DateTime.UtcNow
                            };
                            var createdAccount = await bankAccountService.CreateBankAccountAsync(bankAccount);
                            Console.WriteLine($"Bank account created successfully! Account ID: {createdAccount.Id}, Account Number: {createdAccount.AccountNumber}");
                        }
                        break;

                    case "3":
                        // Show Account Balance
                        int accountId3 = ReadId("Enter Account ID: ");
                        try
                        {
                            decimal balance = await bankAccountService.GetAccountBalanceAsync(accountId3);
                            var account = await bankAccountService.GetAccountInformationAsync(accountId3);
                            Console.WriteLine($"Account Number: {account.AccountNumber}");
                            Console.WriteLine($"Balance: {balance} {account.Currency}");
                            Console.WriteLine($"Status: {account.Status}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "4":
                        // Deposit Money
                        int accountId4 = ReadId("Enter Account ID: ");
                        decimal depositAmount = ReadAmount("Enter Deposit Amount: ");

                        try
                        {
                            string idempotencyKey = Guid.NewGuid().ToString();
                            var transaction = await transactionService.DepositAsync(accountId4, depositAmount, idempotencyKey);
                            Console.WriteLine($"Deposit successful! Transaction ID: {transaction.Id}, Reference: {transaction.ReferenceNumber}");
                            Console.WriteLine($"New Balance: {(await bankAccountService.GetAccountBalanceAsync(accountId4))}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "5":
                        // Withdraw Money
                        int accountId5 = ReadId("Enter Account ID: ");
                        decimal withdrawAmount = ReadAmount("Enter Withdrawal Amount: ");

                        try
                        {
                            string idempotencyKey = Guid.NewGuid().ToString();
                            var transaction = await transactionService.WithdrawAsync(accountId5, withdrawAmount, idempotencyKey);
                            Console.WriteLine($"Withdrawal successful! Transaction ID: {transaction.Id}, Reference: {transaction.ReferenceNumber}");
                            Console.WriteLine($"New Balance: {(await bankAccountService.GetAccountBalanceAsync(accountId5))}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "6":
                        // Transfer Between Accounts
                        int sourceAccountId = ReadId("Enter Source Account ID: ");
                        int destinationAccountId = ReadId("Enter Destination Account ID: ");
                        decimal transferAmount = ReadAmount("Enter Transfer Amount: ");

                        try
                        {
                            string idempotencyKey = Guid.NewGuid().ToString();
                            var transaction = await transactionService.TransferAsync(sourceAccountId, destinationAccountId, transferAmount, idempotencyKey);
                            Console.WriteLine($"Transfer successful! Transaction ID: {transaction.Id}, Reference: {transaction.ReferenceNumber}");
                            Console.WriteLine($"Source Account New Balance: {(await bankAccountService.GetAccountBalanceAsync(sourceAccountId))}");
                            Console.WriteLine($"Destination Account New Balance: {(await bankAccountService.GetAccountBalanceAsync(destinationAccountId))}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "7":
                        // Show Account History
                        int accountId7 = ReadId("Enter Account ID: ");

                        try
                        {
                            var ledgerEntries = await ledgerService.GetLedgerEntriesAsync(accountId7);
                            if (ledgerEntries.Count == 0)
                            {
                                Console.WriteLine("No ledger entries found for this account.");
                            }
                            else
                            {
                                Console.WriteLine("\n===== ACCOUNT HISTORY =====");
                                foreach (var entry in ledgerEntries)
                                {
                                    Console.WriteLine($"ID: {entry.Id}, Amount: {entry.Amount}, Type: {entry.EntryType}, Date: {entry.CreatedAt}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "8":
                        // Freeze Account
                        int accountId8 = ReadId("Enter Account ID to Freeze: ");

                        try
                        {
                            await bankAccountService.FreezeAccountAsync(accountId8);
                            Console.WriteLine("Account frozen successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "9":
                        // Activate Account
                        int accountId9 = ReadId("Enter Account ID to Activate: ");

                        try
                        {
                            await bankAccountService.UnfreezeAccountAsync(accountId9);
                            Console.WriteLine("Account activated successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "10":
                        // Show Transfer Limits
                        int customerId10 = ReadId("Enter Customer ID: ");

                        try
                        {
                            var customer = await customerService.GetCustomerAsync(customerId10);
                            var allLimits = await context.TransferLimits
                                .Where(tl => tl.CustomerId == customerId10)
                                .ToListAsync();

                            if (allLimits.Count == 0)
                            {
                                Console.WriteLine("No transfer limits found for this customer.");
                            }
                            else
                            {
                                Console.WriteLine("\n===== TRANSFER LIMITS =====");
                                Console.WriteLine($"Customer: {customer.FullName}");
                                foreach (var limit in allLimits)
                                {
                                    Console.WriteLine($"\nCurrency: {limit.Currency}");
                                    Console.WriteLine($"Single Transaction Limit: {limit.SingleTransactionLimit}");
                                    Console.WriteLine($"Daily Transfer Limit: {limit.DailyLimit}");
                                    Console.WriteLine("---");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "11":
                        // Financial Reports
                        int customerId11 = ReadId("Enter Customer ID: ");

                        try
                        {
                            var customer = await customerService.GetCustomerAsync(customerId11);
                            var totalBalance = await customerService.GetCustomerTotalBalanceAsync(customerId11);
                            var balanceByCurrency = await customerService.GetCustomerTotalBalanceByCurrencyAsync(customerId11);

                            Console.WriteLine("\n===== FINANCIAL REPORT =====");
                            Console.WriteLine($"Customer: {customer.FullName}");
                            Console.WriteLine($"Total Balance (All Currencies): {totalBalance}");
                            Console.WriteLine("\nBalance by Currency:");
                            foreach (var kvp in balanceByCurrency)
                            {
                                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "12":
                        // Ledger Audit
                        int accountId12 = ReadId("Enter Account ID for Audit: ");

                        try
                        {
                            var account = await bankAccountService.GetAccountInformationAsync(accountId12);
                            var calculatedBalance = await ledgerService.CalculateBalanceFromLedgerAsync(accountId12);
                            bool hasDiscrepancy = await ledgerService.HasBalanceDiscrepancyAsync(accountId12);

                            Console.WriteLine("\n===== LEDGER AUDIT REPORT =====");
                            Console.WriteLine($"Account: {account.AccountNumber}");
                            Console.WriteLine($"Current Balance: {account.Balance}");
                            Console.WriteLine($"Calculated Balance from Ledger: {calculatedBalance}");
                            Console.WriteLine($"Has Discrepancy: {hasDiscrepancy}");

                            if (!hasDiscrepancy)
                            {
                                Console.WriteLine("✓ Account is balanced correctly!");
                            }
                            else
                            {
                                Console.WriteLine("✗ WARNING: Balance discrepancy detected!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "0":
                        Console.WriteLine("Application closed.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }

    private static int ReadId(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
            {
                return id;
            }

            Console.WriteLine(
                "ID must be a positive integer. Please try again.");
        }
    }

    private static decimal ReadAmount(string message)
    {
        while (true)
        {
            Console.Write(message);

            string? input = Console.ReadLine();

            bool parsed = decimal.TryParse(
                input,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out decimal amount);

            if (parsed && amount > 0)
            {
                return amount;
            }

            Console.WriteLine("Amount must be a positive decimal number. Please try again.");
        }
    }
}