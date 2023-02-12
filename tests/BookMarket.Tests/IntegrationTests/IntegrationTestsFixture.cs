using Microsoft.AspNetCore.TestHost;

namespace BookMarket.Tests.IntegrationTests;

public class IntegrationTestsFixture : IDisposable
{
    public readonly TestServer Server;

    public IntegrationTestsFixture()
    {
        var webHost = Util.BuildTestWebHost(_ =>
        {
            
        });
        Server = new TestServer(webHost);
    }

    public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
    {
        using var client = Server.CreateClient();
        return await client.SendAsync(request);
    }

    public void Dispose()
    {
        Server?.Dispose();
    }
}