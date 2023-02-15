namespace Application.Books.Responses;

public sealed class BooksResponse
{
    public Book[] Books { get; set; } = Array.Empty<Book>();
}