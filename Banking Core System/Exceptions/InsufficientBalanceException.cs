namespace Banking_Core_System.Exceptions;

public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(string message)
        : base(message)
    {
    }
}
