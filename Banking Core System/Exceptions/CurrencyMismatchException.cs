namespace Banking_Core_System.Exceptions;

public class CurrencyMismatchException : Exception
{
    public CurrencyMismatchException(string message)
        : base(message)
    {
    }
}
