using Application.Abstractions;
using Application.Books.Responses;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Books.Queries.GetAllBooks;

public class GetAllBooksQueryHandler : IQueryHandler<GetAllBooksQuery, BooksResponse>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetAllBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }
    
    public async Task<BooksResponse> Handle(GetAllBooksQuery request, CancellationToken ct)
    {
        var books = await _bookRepository.GetAll(ct);

        var booksResp = books
            .Select(x => _mapper.Map<Book, BookResponse>(x))
            .ToArray();

        return new BooksResponse{ Books = booksResp};
    }
}