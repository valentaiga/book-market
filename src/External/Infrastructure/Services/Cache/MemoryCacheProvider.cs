using Application.Abstractions.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Cache;

public class MemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheProvider(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public ValueTask<CachedValue<T?>> Get<T>(string cacheKey, CancellationToken ct)
    {
        var value = _memoryCache.Get<T?>(cacheKey);
        var cachedValue = value is null
            ? CachedValue<T>.NotFound()
            : CachedValue<T?>.FromCache(value);

        return ValueTask.FromResult(cachedValue);
    }

    public ValueTask Save<T>(string cacheKey, T? value)
    {
        _memoryCache.Set<T?>(cacheKey, value);
        return ValueTask.CompletedTask;
    }

    public ValueTask Delete(string cacheKey)
    {
        _memoryCache.Remove(cacheKey);
        return ValueTask.CompletedTask;
    }
}