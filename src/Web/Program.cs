using System.Data;
using Application.Behaviors;
using Domain.Abstractions;
using FluentMigrator.Runner;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.WithTraceIdentifier()
    .WriteTo.Console()
    .CreateLogger();

var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;
var applicationAssembly = typeof(Application.ApplicationAssembly).Assembly;

builder.Services.AddControllers()
    .AddApplicationPart(presentationAssembly);

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddSingleton<IDbConnectionFactory, PgsqlConnectionFactory>();
builder.Services.AddScoped<IDbConnection>(src => src.GetRequiredService<IDbConnectionFactory>().GetConnection());
// builder.Services.AddScoped<IDbTransaction>(services => 
//     services.GetRequiredService<UnitOfWork>().Transaction);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.AddMediatR(applicationAssembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var file = $"{presentationAssembly.GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, file);
    c.IncludeXmlComments(filePath);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" });
});

// todo: structure all this mess
if (args.Contains("-migrate"))
{
#pragma warning disable ASP0000
    await using var serviceProvider = new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(rb =>
            rb.AddPostgres()
                .WithGlobalConnectionString(builder.Configuration.GetConnectionString("PostgresDb"))
                .ScanIn(applicationAssembly).For.Migrations())
        .AddLogging(b => b.AddFluentMigratorConsole())
        .BuildServiceProvider();
#pragma warning restore ASP0000
    
    var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();
    
    if (args.Contains("down"))
        migrationRunner.MigrateDown(0);
    else
        migrationRunner.MigrateUp();

    return;
}

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

await app.RunAsync();