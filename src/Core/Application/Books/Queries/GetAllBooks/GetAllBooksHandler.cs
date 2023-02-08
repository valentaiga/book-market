using Application.Abstractions;
using Application.Books.Queries.GetBookById;
using Domain.Abstractions;

namespace Application.Books.Queries.GetAllBooks;

public class GetAllBooksHandler : IQueryHandler<GetAllBooksQuery, BooksResponse>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<BooksResponse> Handle(GetAllBooksQuery request, CancellationToken ct)
    {
        var books = await _bookRepository.GetAll(ct);

        // adapt
        var booksResp = books
            .Select(x => new BookResponse { Id = x.Id })
            .ToArray();

        return new BooksResponse{ Books = booksResp};
    }
}