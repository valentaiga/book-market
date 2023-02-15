using System.Data;
using Application.Behaviors;
using Domain.Abstractions;
using FluentValidation;
using Infrastructure;
using Infrastructure.Services.Repositories;
using Mapster;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using IMapper = Application.Abstractions.IMapper;

namespace Web.Extensions;

public static class DependencyInjectionExtensions
{
    public static void ConfigureControllers(this IServiceCollection services)
    {
        var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

        services.AddControllers()
            .AddApplicationPart(presentationAssembly);
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var file = $"{presentationAssembly.GetName().Name}.xml";
            var filePath = Path.Combine(AppContext.BaseDirectory, file);
            c.IncludeXmlComments(filePath);
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        });
    }

    public static void ConfigureMediatR(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Application.ApplicationAssembly).Assembly;
        services.AddMediatR(applicationAssembly);
    }

    public static void ConfigureValidation(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Application.ApplicationAssembly).Assembly;

        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }    
    
    public static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddSingleton<TypeAdapterConfig>(_ => new TypeAdapterConfig());
        services.AddSingleton<MapsterMapper.IMapper, MapsterMapper.Mapper>();
        services.AddSingleton<IMapper, InternalMapper>();
    }

    public static void ConfigureDatabase(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services.AddSingleton<IDbConnectionFactory, PgsqlConnectionFactory>();
        services.AddScoped<IDbConnection>(src => 
            src.GetRequiredService<IDbConnectionFactory>().GetConnection());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
    }

    public static void ConfigureLogger(this ILoggingBuilder builder)
    {
        var config = new LoggerConfiguration()
            .Enrich.WithTraceIdentifier()
            .WriteTo.Console();

        Log.Logger = config.CreateLogger();
    }
}