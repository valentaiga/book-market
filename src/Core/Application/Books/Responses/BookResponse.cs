namespace Application.Books.Responses;

public sealed class BookResponse
{
    public Guid Id { get; set; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? PublishDate { get; init; }
    public int PagesCount { get; init; }
    public string? Language { get; init; }
    public Guid AuthorId { get; init; }
}