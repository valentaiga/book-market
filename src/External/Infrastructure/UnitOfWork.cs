using System.Data;
using Domain.Abstractions;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IDbConnection _dbConnection;
    private IDbTransaction? _dbTransaction;

    public UnitOfWork(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public IDbTransaction Transaction => _dbTransaction ??= _dbConnection.BeginTransaction();

    public void Commit()
    {
        _dbTransaction?.Commit();
    }

    public void Rollback()
    {
        _dbTransaction?.Rollback();
    }

    public void Dispose()
    {
        _dbTransaction?.Dispose();
    }
}