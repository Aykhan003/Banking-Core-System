namespace Banking_Core_System.Exceptions;

public class AccountClosedException : Exception
{
    public AccountClosedException(string message)
        : base(message)
    {
    }
}
