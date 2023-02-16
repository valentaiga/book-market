using Application.Abstractions;
using Application.Authors.Responses;
using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Application.Authors.Queries.GetAllAuthors;

public class GetAllAuthorsQueryHandler : IQueryHandler<GetAllAuthorsQuery, AuthorsResponse>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<AuthorsResponse> Handle(GetAllAuthorsQuery request, CancellationToken ct)
    {
        var authors = await _authorRepository.GetAll(ct);

        var authorsResp = authors
            .Select(x => _mapper.Map<AuthorDto, Author>(x))
            .ToArray();

        return new AuthorsResponse { Authors = authorsResp };
    }
}