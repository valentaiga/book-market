using System.Data;

namespace Domain.Abstractions;

public interface IUnitOfWork
{
    IDbTransaction Transaction { get; }
    void Commit();
    void Rollback();
}