using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var stage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Development";

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{stage}.json")
    .Build();

using var serviceProvider = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(rb =>
        rb.AddPostgres()
            .WithGlobalConnectionString(configuration.GetConnectionString("PostgresDb"))
            .ScanIn(typeof(Program).Assembly).For.Migrations())
    .AddLogging(b => b.AddFluentMigratorConsole())
    .BuildServiceProvider();

var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();

if (args.Contains("-down"))
    migrationRunner.MigrateDown(0);
else if (args.Contains("-up"))
    migrationRunner.MigrateUp();