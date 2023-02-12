namespace Domain.Shared;

public class Result
{
    public Error? Error { get; }
    public bool IsError => Error is not null;

    public Result(Error error)
        => Error = error;

    public Result()
    {
    }

    public static Result Success() => new();
    public static Result<TResult> Success<TResult>(TResult data) => new(data);
    public static Result Failure(string message, string traceId) => new(new Error(message, traceId));
    public static Result Failure<TDetails>(string message, string traceId, TDetails details) =>
        new(new ErrorDetailed<TDetails>(message, traceId, details));
}