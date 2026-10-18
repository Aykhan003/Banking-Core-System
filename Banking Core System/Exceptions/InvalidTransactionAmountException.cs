namespace Banking_Core_System.Exceptions;

public class InvalidTransactionAmountException : Exception
{
    public InvalidTransactionAmountException(string message)
        : base(message)
    {
    }
}
