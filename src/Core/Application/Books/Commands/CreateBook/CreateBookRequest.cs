namespace Application.Books.Commands.CreateBook;

public record CreateBookRequest(
    string Title,
    string Description,
    DateTime PublishDate,
    int PagesCount,
    string Language,
    Guid AuthorId);