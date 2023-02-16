using Application.Abstractions;
using Application.Authors.Responses;
using Application.Books.Responses;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IQueryHandler<GetBookByIdQuery, Book>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(
        IBookRepository bookRepository, 
        IAuthorRepository authorRepository, 
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken ct)
    {
        var dto = await _bookRepository.GetById(request.BookId, ct);
        
        if (dto is null)
            throw new BookNotFoundException(request.BookId);

        var authorDto = await _authorRepository.GetById(dto.AuthorId, ct); 
        
        var book = _mapper.Map<BookDto, Book>(dto);
        book.Author = _mapper.Map<AuthorDto, Author>(authorDto!);
        
        return book;
    } 
}