namespace Application.Books.Responses;

public sealed class BooksResponse
{
    public BookResponse[] Books { get; set; } = Array.Empty<BookResponse>();
}