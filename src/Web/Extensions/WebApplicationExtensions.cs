using Web.Middleware;

namespace Web.Extensions;

public static class WebApplicationExtensions
{
    public static void AddMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    public static void AddSwagger(this IApplicationBuilder app)
    {
        var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}