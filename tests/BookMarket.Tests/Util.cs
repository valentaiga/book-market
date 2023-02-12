using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Web.Extensions;

namespace BookMarket.Tests;

public static class Util
{
    public static IWebHostBuilder BuildTestWebHost(Action<IServiceCollection> testServicesConfiguration)
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT","Testing");
        
        return WebHost.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.ConfigureMediatR();
                services.ConfigureMapper();
                services.ConfigureDatabase();
                services.ConfigureValidation();
                services.ConfigureControllers();
            })
            .ConfigureTestServices(testServicesConfiguration)
            .Configure(app =>
            {
                app.AddMiddleware();
                app.AddSwagger();
            });
    }
}