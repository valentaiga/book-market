using System.Data;
using System.Data.Common;
using Dapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Architecture;

namespace Infrastructure.Services.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorRepository(IDbConnection dbConnection, IUnitOfWork unitOfWork)
    {
        _dbConnection = dbConnection;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AuthorDto[]> GetAll(CancellationToken ct)
    {
        const string query = @"SELECT * FROM authors";
        
        try
        {
            var command = new CommandDefinition(query, cancellationToken: ct);
            var result = await _dbConnection.QueryAsync<AuthorDto>(command);
            return result.ToArray();
        }
        catch (DbException ex)
        {
            throw new DatabaseException($"Get all authors query failed", ex);
        }
    }

    public async Task<AuthorDto> GetById(Guid authorId, CancellationToken ct)
    {
        const string query = @"SELECT * FROM authors WHERE id = @AuthorId LIMIT 1";
        
        try
        {
            var command = new CommandDefinition(query, new { authorId }, cancellationToken: ct);
            return await _dbConnection.QueryFirstOrDefaultAsync<AuthorDto>(command);
        }
        catch (DbException ex)
        {
            throw new DatabaseException($"Get author by id:'{authorId}' failed", ex);
        }
    }

    public async Task<Guid> Insert(AuthorDto author)
    {
        var query = @"INSERT INTO authors
(name)
VALUES (@Name)
RETURNING id;";
        try
        {
            var command = new CommandDefinition(query, new { author.Name }, transaction: _unitOfWork.Transaction); 
            return await _dbConnection.ExecuteScalarAsync<Guid>(command);
        }
        catch (DbException ex)
        {
            throw new DatabaseException($"Insert author name:'{author.Name}' failed", ex);
        }
    }

    public async Task<bool> Delete(Guid authorId)
    {
        const string query = @"DELETE FROM authors WHERE id = @authorId";
        
        try
        {
            var command = new CommandDefinition(query, new { authorId }, transaction: _unitOfWork.Transaction);
            var result = await _dbConnection.ExecuteAsync(command);
            _unitOfWork.Commit();
            return result == 1;
        }
        catch (DbException ex)
        {
            throw new DatabaseException($"Author delete by id:'{authorId}' failed", ex);
        }
    }
}