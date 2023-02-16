namespace Application.Abstractions.Cache;

public interface ICacheProvider
{
    ValueTask<CachedValue<T?>> Get<T>(string cacheKey, CancellationToken ct);
    ValueTask Save<T>(string cacheKey, T? value);
    ValueTask Delete(string cacheKey);
}