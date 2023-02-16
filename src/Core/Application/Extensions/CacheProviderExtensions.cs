using Application.Abstractions.Cache;

namespace Application.Extensions;

public static class CacheProviderExtensions
{
    public static async Task<T> GetOrSet<T>(this ICacheProvider cacheProvider, string cacheKey, Func<Task<T>> get, CancellationToken ct = default)
    {
        var cache = await cacheProvider.Get<T>(cacheKey, ct);
        if (cache.FoundInCache) return cache.Value!;
        var value = await get.Invoke();
        await cacheProvider.Save(cacheKey, value);
        return value;
    }
}