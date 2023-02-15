namespace Domain.Shared;

public sealed class Result<TResult> : Result
{
    public TResult Data { get; init; }

    public Result(TResult data)
        => Data = data;
}