using Application.Abstractions;
using Application.Books.Responses;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IQueryHandler<GetBookByIdQuery, BookResponse>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<BookResponse> Handle(GetBookByIdQuery request, CancellationToken ct)
    {
        var book = await _bookRepository.GetById(request.BookId, ct);
        
        if (book is null)
            throw new BookNotFoundException(request.BookId);

        return _mapper.Map<Book, BookResponse>(book);
    } 
}