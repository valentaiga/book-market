using Application.Abstractions;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Shared;
using IMapper = Application.Abstractions.IMapper;

namespace Application.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, Result<Guid>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateBookCommand request, CancellationToken ct)
    {
        var author = await _authorRepository.GetById(request.AuthorId, ct);
        if (author is null)
            throw new AuthorNotFoundException(request.AuthorId);

        var alreadyExists = await _bookRepository.ExistsByTitleAndAuthor(request.Title, request.AuthorId, ct);
        if (alreadyExists)
            throw new BookDuplicateException(request.Title);

        var book = _mapper.Map<CreateBookCommand, BookDto>(request);

        var bookId = await _bookRepository.Insert(book);
        _unitOfWork.Commit();
        
        return Result.Success(bookId);
    }
}