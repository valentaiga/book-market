using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace BookMarket.Tests;

internal static class Util
{
    public static WebApplicationFactory<Program> BuildTestServer(Action<IServiceCollection> testServicesConfiguration)
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        var webHost = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b
                .ConfigureServices(testServicesConfiguration)
            .UseTestServer());
        return webHost;
    }
    
    public const string SymbolsCount21 = "123456789012345678901";
    public const string SymbolsCount61 = "1234567890123456789012345678901234567890123456789012345678901";
    public const string SymbolsCount513 = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123";
}