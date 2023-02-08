using System.Data;
using Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure;

public class PgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    
    public PgsqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PostgresDb") ??
            throw new ApplicationException("PostgresDb connection string is not set.");
    }
    
    public IDbConnection GetConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}