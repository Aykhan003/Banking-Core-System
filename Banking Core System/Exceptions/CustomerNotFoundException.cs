namespace Banking_Core_System.Exceptions;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(string message) : base(message)
    {
    }
}
