using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;

namespace Application.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, Result<Guid>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateBookCommand request, CancellationToken ct)
    {
        var book = new Book(
            request.Title,
            request.Description,
            request.PublishDate,
            request.PagesCount,
            request.Language,
            request.AuthorId);

        var bookId = await _bookRepository.Insert(book);
        _unitOfWork.Commit();
        
        return Result.Success(bookId);
    }
}