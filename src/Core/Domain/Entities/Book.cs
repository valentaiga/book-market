using Domain.Primitives;

namespace Domain.Entities;

public class Book : Entity
{
    private Book()
    {
        Title = null;
        Description = null;
    }
    
    public Book(
        string title,
        string description,
        DateTime publishDate,
        int pagesCount,
        string language,
        Guid authorId)
    {
        Title = title;
        Description = description;
        PublishDate = publishDate;
        PagesCount = pagesCount;
        Language = language;
        AuthorId = authorId;
    }

    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? PublishDate { get; init; }
    public int PagesCount { get; init; }
    public string? Isbn10 { get; init; }
    public string? Isbn13 { get; init; }
    public string? Language { get; init; }
    public Guid AuthorId { get; init; }
}