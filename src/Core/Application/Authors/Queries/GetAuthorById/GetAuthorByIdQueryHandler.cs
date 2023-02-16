using Application.Abstractions;
using Application.Authors.Responses;
using Application.Books.Responses;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Authors.Queries.GetAuthorById;

public class GetAuthorByIdQueryHandler : IQueryHandler<GetAuthorByIdQuery, Author>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetAuthorByIdQueryHandler(
        IAuthorRepository authorRepository, 
        IBookRepository bookRepository,
        IMapper mapper)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken ct)
    {
        var dto = await _authorRepository.GetById(request.AuthorId, ct);
        
        if (dto is null)
            throw new AuthorNotFoundException(request.AuthorId);

        var books = await _bookRepository.GetByAuthor(dto.Id, ct);
        var author = _mapper.Map<AuthorDto, Author>(dto);
        author.Books = books.Select(x => _mapper.Map<BookDto, Book>(x)).ToArray();

        return author;
    } 
}