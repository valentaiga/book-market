namespace Domain.Exceptions.Base;

public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message)
    {
    }
}