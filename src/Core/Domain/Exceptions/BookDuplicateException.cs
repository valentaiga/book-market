using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class BookDuplicateException : BadRequestException
{
    public BookDuplicateException(string title) 
        : base($"The book with title '{title}' already exists")
    {
    }
}