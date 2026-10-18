namespace Banking_Core_System.Exceptions;

public class TransferLimitExceededException : Exception
{
    public TransferLimitExceededException(string message)
        : base(message)
    {
    }
}
