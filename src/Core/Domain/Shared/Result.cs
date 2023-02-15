using System.Text.Json.Serialization;

namespace Domain.Shared;

public class Result
{
    public bool IsError { get; init; }

    [JsonConstructor]
    public Result(bool isError)
        => IsError = isError;

    public static Result Success() => new(false);
    public static Result<TResult> Success<TResult>(TResult data) => new(data, false);
    public static Result<Error> Failure(string message, string traceId) => new(new Error(message, traceId), true);
    public static Result<ErrorDetailed<TDetails>> Failure<TDetails>(string message, string traceId, TDetails details) =>
        new(new ErrorDetailed<TDetails>(message, traceId, details), true);
}