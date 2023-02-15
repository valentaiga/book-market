using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;
using IMapper = Application.Abstractions.IMapper;

namespace Application.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, Result<Guid>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(
        IBookRepository bookRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateBookCommand request, CancellationToken ct)
    {
        var book = _mapper.Map<CreateBookCommand, BookDto>(request);

        var bookId = await _bookRepository.Insert(book);
        _unitOfWork.Commit();
        
        return Result.Success(bookId);
    }
}