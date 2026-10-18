namespace Banking_Core_System.Exceptions;

public class TransactionNotFoundException : Exception
{
    public TransactionNotFoundException(string message)
        : base(message)
    {
    }
}
