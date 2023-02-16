using Application.Abstractions;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Domain.Shared;

namespace Application.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand, Result>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var success = await _bookRepository.Delete(request.BookId);

        if (!success)
        {
            _unitOfWork.Rollback();
            throw new BookNotFoundException(request.BookId);
        }
        
        _unitOfWork.Commit();
        return Result.Success();
    }
}