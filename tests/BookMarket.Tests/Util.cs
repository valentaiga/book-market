using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace BookMarket.Tests;

public static class Util
{
    internal static WebApplicationFactory<Program> BuildTestServer(Action<IServiceCollection> testServicesConfiguration)
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        var webHost = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b
                .ConfigureServices(testServicesConfiguration)
            .UseTestServer());
        return webHost;
    }
}