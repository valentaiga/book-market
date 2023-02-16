using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public sealed class AuthorNotFoundException : BadRequestException
{
    public AuthorNotFoundException(Guid authorId)
        : base($"The author with the identifier {authorId} not found")
    {
    }
}