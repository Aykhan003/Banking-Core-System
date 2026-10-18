namespace Banking_Core_System.Exceptions;

public class DuplicateIdempotencyKeyException : Exception
{
    public DuplicateIdempotencyKeyException(string message)
        : base(message)
    {
    }
}
