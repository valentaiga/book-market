using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public sealed class BookNotFoundException : BadRequestException
{
    public BookNotFoundException(Guid bookId)
        : base($"The book with the identifier {bookId} not found")
    {
    }
}