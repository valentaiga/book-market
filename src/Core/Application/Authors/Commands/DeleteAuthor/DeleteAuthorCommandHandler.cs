using Application.Abstractions;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Shared;

namespace Application.Authors.Commands.DeleteAuthor;

public class DeleteAuthorCommandHandler : ICommandHandler<DeleteAuthorCommand, Result>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAuthorCommandHandler(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var success = await _authorRepository.Delete(request.AuthorId);

        if (!success)
        {
            _unitOfWork.Rollback();
            throw new AuthorNotFoundException(request.AuthorId);
        }
        
        _unitOfWork.Commit();
        return Result.Success();
    }
}