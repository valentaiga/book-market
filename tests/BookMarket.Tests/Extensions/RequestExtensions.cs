using System.Text.Json;

namespace BookMarket.Tests.Extensions;

public static class RequestExtensions
{
    public static async Task<TResponse> DeserializeAsync<TResponse>(this HttpResponseMessage resp)
    {
        await using var stream = await resp.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<TResponse>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
}