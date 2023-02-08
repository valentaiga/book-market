using Domain.Entities;

namespace Domain.Abstractions;

public interface IBookRepository
{
    Task<Book[]> GetAll(CancellationToken ct);
    Task<Book> GetById(Guid bookId, CancellationToken ct);
    Task<Guid> Insert(Book book);
    Task<bool> ExistsByTitleAndAuthor(string title, Guid authorId, CancellationToken ct);
}