namespace Domain.Shared;

public sealed class Result<TResult> : Result
{
    public TResult Data { get; init; }

    public Result(TResult data, bool isError) : base(isError)
        => (Data) = (data);
}