using System.Data;

namespace Domain.Abstractions;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}