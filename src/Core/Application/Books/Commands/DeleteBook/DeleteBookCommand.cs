using Application.Abstractions;
using Domain.Shared;

namespace Application.Books.Commands.DeleteBook;

public sealed record DeleteBookCommand(Guid BookId) : ICommand<Result>;