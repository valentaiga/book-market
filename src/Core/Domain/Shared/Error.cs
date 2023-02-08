namespace Domain.Shared;

public class Error
{
    public string Message { get; }
    public string TraceId { get; }

    public Error(string message, string traceId) => (Message, TraceId) = (message, traceId);
}