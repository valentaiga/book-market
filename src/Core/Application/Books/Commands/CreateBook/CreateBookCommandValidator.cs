using FluentValidation;

namespace Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(60);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(512);
        RuleFor(x => x.PublishDate).NotEmpty();
        RuleFor(x => x.PagesCount).NotEmpty();
        RuleFor(x => x.Language).NotEmpty().MaximumLength(20);
        RuleFor(x => x.AuthorId).NotEmpty();
    }
}