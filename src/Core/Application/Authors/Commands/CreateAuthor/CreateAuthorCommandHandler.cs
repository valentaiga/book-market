using Application.Abstractions;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared;
using IMapper = Application.Abstractions.IMapper;

namespace Application.Authors.Commands.CreateAuthor;

internal sealed class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand, Result<Guid>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAuthorCommandHandler(
        IAuthorRepository authorRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAuthorCommand request, CancellationToken ct)
    {
        var author = _mapper.Map<CreateAuthorCommand, AuthorDto>(request);

        var authorId = await _authorRepository.Insert(author);
        _unitOfWork.Commit();
        
        return Result.Success(authorId);
    }
}