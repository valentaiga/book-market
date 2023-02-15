using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BookMarket.Tests.Extensions;

public static class RequestExtensions
{
    public static async Task<TResponse> DeserializeAsync<TResponse>(this HttpResponseMessage resp)
    {
        await using var stream = await resp.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<TResponse>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    internal static async Task<HttpResponseMessage> SendRequestAsync(this WebApplicationFactory<Program> server, HttpRequestMessage request)
    {
        using var client = server.CreateClient();
        return await client.SendAsync(request);
    }
}