using Domain.Exceptions.Base;

namespace Domain.Exceptions.Architecture;

public sealed class DatabaseException : InternalErrorException
{
    public DatabaseException(string message, Exception ex) : base(message, ex)
    {
    }
}