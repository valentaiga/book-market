using Web.Middleware;

namespace Web.Extensions;

public static class WebApplicationExtensions
{
    public static void AddMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}