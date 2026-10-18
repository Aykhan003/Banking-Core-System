namespace Banking_Core_System.Exceptions;

public class AccountFrozenException : Exception
{
    public AccountFrozenException(string message)
        : base(message)
    {
    }
}
