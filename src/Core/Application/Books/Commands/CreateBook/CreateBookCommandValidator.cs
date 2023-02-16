using FluentValidation;

namespace Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithErrorCode(BookValidationErrors.TitleInvalid)
            .MaximumLength(60).WithErrorCode(BookValidationErrors.TitleInvalid);
        RuleFor(x => x.Description)
            .NotEmpty().WithErrorCode(BookValidationErrors.DescriptionInvalid)
            .MaximumLength(512).WithErrorCode(BookValidationErrors.DescriptionInvalid);
        RuleFor(x => x.PublishDate)
            .NotEmpty().WithErrorCode(BookValidationErrors.PublishDateInvalid);
        RuleFor(x => x.PagesCount)
            .NotEmpty().WithErrorCode(BookValidationErrors.PagesCountInvalid);
        RuleFor(x => x.Language)
            .NotEmpty().WithErrorCode(BookValidationErrors.LanguageInvalid)
            .MaximumLength(20).WithErrorCode(BookValidationErrors.LanguageInvalid);
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithErrorCode(BookValidationErrors.AuthorIdInvalid);
    }
}