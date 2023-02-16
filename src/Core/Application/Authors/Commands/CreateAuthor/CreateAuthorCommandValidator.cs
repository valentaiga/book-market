using FluentValidation;

namespace Application.Authors.Commands.CreateAuthor;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode(AuthorValidationErrors.NameInvalid)
            .MaximumLength(60).WithErrorCode(AuthorValidationErrors.NameInvalid);
    }
}