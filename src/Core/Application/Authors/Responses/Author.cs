using Application.Books.Responses;

namespace Application.Authors.Responses;

public sealed class Author
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    
    public Book[] Books { get; set; }
}