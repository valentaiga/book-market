namespace Application.Abstractions.Cache;

public sealed class CachedValue<T>
{
    private CachedValue(T? value, bool foundInCache)
    {
        Value = value;
        FoundInCache = foundInCache;
    }
    
    public T? Value { get; }
    public bool FoundInCache { get; }
    
    public static CachedValue<T?> FromCache(T? value)
        => new CachedValue<T?>(value, true);

    public static CachedValue<T?> NotFound()
        => new CachedValue<T?>(default, false);
}