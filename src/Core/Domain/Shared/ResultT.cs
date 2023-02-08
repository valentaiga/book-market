namespace Domain.Shared;

public sealed class Result<TResult> : Result
{
    public TResult Data { get; }

    public Result(TResult data)
        => Data = data;
}