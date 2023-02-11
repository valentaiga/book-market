using Application.Abstractions;
using Application.Books.Responses;
using Domain.Abstractions;
using Domain.Exceptions;

namespace Application.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IQueryHandler<GetBookByIdQuery, BookResponse>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponse> Handle(GetBookByIdQuery request, CancellationToken ct)
    {
        var book = await _bookRepository.GetById(request.BookId, ct);
        
        if (book is null)
            throw new BookNotFoundException(request.BookId);

        //var resp = book.Adapt<BookResponse>();
        var resp = new BookResponse
        {
            Id = book.Id
        };
        return resp;
    } 
}