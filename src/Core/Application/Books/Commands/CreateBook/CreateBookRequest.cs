namespace Application.Books.Commands.CreateBook;

public sealed record CreateBookRequest(
    string Title,
    string Description,
    DateTime PublishDate,
    short PagesCount,
    string Language,
    Guid AuthorId);