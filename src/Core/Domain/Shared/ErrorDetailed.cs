namespace Domain.Shared;

public sealed class ErrorDetailed<T> : Error
{
    public T Details { get; }

    public ErrorDetailed(string message, string traceId, T details) : base(message, traceId)
        => Details = details;
}