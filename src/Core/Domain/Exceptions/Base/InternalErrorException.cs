namespace Domain.Exceptions.Base;

public class InternalErrorException : Exception
{
    public Exception InternalException { get; }
    
    public InternalErrorException(string message, Exception ex)
        : base(message)
    {
        InternalException = ex;
    }
}