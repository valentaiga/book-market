using Domain.Primitives;

namespace Domain.Entities;

public class BookDto : Entity
{
    public BookDto()
    {
        Title = null;
        Description = null;
    }

    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? PublishDate { get; init; }
    public short PagesCount { get; init; }
    public string? Language { get; init; }
    public Guid AuthorId { get; init; }
}