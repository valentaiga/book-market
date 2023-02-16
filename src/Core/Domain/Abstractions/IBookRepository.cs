using Domain.Entities;

namespace Domain.Abstractions;

public interface IBookRepository
{
    Task<BookDto[]> GetAll(CancellationToken ct);
    Task<BookDto?> GetById(Guid bookId, CancellationToken ct);
    Task<BookDto[]> GetByAuthor(Guid authorId, CancellationToken ct);
    Task<Guid> Insert(BookDto book);
    Task<bool> Delete(Guid bookId);
    Task<bool> ExistsByTitleAndAuthor(string title, Guid authorId, CancellationToken ct);
}