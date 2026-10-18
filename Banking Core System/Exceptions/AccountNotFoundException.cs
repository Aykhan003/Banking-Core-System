namespace Banking_Core_System.Exceptions;

public class AccountNotFoundException : Exception
{
   public AccountNotFoundException(string message)
        : base(message)
    {
    }
}
