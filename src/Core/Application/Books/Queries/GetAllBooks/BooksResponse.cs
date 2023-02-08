using Application.Books.Queries.GetBookById;

namespace Application.Books.Queries.GetAllBooks;

public sealed class BooksResponse
{
    public BookResponse[] Books { get; set; } = Array.Empty<BookResponse>();
}