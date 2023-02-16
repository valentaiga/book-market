using Application.Abstractions;
using Application.Authors.Responses;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Authors.Queries.GetAuthorById;

public class GetAuthorByIdQueryHandler : IQueryHandler<GetAuthorByIdQuery, Author>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken ct)
    {
        var author = await _authorRepository.GetById(request.AuthorId, ct);
        
        if (author is null)
            throw new AuthorNotFoundException(request.AuthorId);

        return _mapper.Map<AuthorDto, Author>(author);
    } 
}