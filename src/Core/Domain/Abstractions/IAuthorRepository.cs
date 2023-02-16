using Domain.Entities;

namespace Domain.Abstractions;

public interface IAuthorRepository
{
    Task<AuthorDto[]> GetAll(CancellationToken ct);
    Task<AuthorDto?> GetById(Guid authorId, CancellationToken ct);
    Task<Guid> Insert(AuthorDto author);
    Task<bool> Delete(Guid authorId);
}