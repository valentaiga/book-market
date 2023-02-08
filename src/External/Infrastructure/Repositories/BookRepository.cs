using System.Data;
using Dapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Architecture;

namespace Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IUnitOfWork _unitOfWork;

    public BookRepository(IDbConnection dbConnection, IUnitOfWork unitOfWork)
    {
        _dbConnection = dbConnection;
        _unitOfWork = unitOfWork;
    }

    public async Task<Book> GetById(Guid bookId, CancellationToken ct)
    {
        const string query = @"SELECT * FROM books WHERE ID = @BookId LIMIT 1";
        
        try
        {
            var command = new CommandDefinition(query, new { bookId }, cancellationToken: ct);
            return await _dbConnection.QueryFirstOrDefaultAsync<Book>(command);
        }
        catch (Exception ex)
        {
            throw new DatabaseException($"Get book by id:'{bookId}' failed", ex);
        }
    }

    public async Task<Book[]> GetAll(CancellationToken ct)
    {
        const string query = @"SELECT * FROM books";
        
        try
        {
            var command = new CommandDefinition(query, cancellationToken: ct);
            var result = await _dbConnection.QueryAsync<Book>(command);
            return result.ToArray();
        }
        catch (Exception ex)
        {
            throw new DatabaseException($"Get all books query failed", ex);
        }
    }

    public async Task<Guid> Insert(Book book)
    {
        var query = @"INSERT INTO books
(title, description, publish_date, pages_count, author_id, language)
VALUES (@Title, @Description, @PublishDate, @PagesCount, @AuthorId, @Language)
RETURNING id;";
        try
        {
            var command = new CommandDefinition(query, new
                {
                    book.Title,
                    book.Description,
                    book.PublishDate,
                    book.PagesCount,
                    book.AuthorId,
                    book.Language
                },
                transaction: _unitOfWork.Transaction); 
            return await _dbConnection.ExecuteScalarAsync<Guid>(command);
        }
        catch (Exception ex)
        {
            throw new DatabaseException($"Insert book title:'{book.Title}' failed", ex);
        }
    }

    public async Task<bool> ExistsByTitleAndAuthor(string title, Guid authorId, CancellationToken ct)
    {
        var query = @"SELECT EXISTS(SELECT * FROM books WHERE title = @Title AND authorId = @AuthorId)";
        try
        {
            var command = new CommandDefinition(query, new { title, authorId }, cancellationToken: ct);
            return await _dbConnection.ExecuteScalarAsync<bool>(command);
        }
        catch (Exception ex)
        {
            throw new DatabaseException($"Book existence check title:'{title}' failed", ex);
        }
    }
}