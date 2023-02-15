using Domain.Primitives;

namespace Domain.Entities;

public class AuthorDto : Entity
{
    public AuthorDto()
    {
        Name = null;
    }

    public string? Name { get; init; }
}