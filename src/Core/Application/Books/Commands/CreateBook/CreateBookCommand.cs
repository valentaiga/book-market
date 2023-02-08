using Application.Abstractions;
using Domain.Shared;

namespace Application.Books.Commands.CreateBook;

public sealed record CreateBookCommand(
    string Title,
    string Description,
    DateTime PublishDate,
    int PagesCount,
    string Language,
    Guid AuthorId) : ICommand<Result<Guid>>;