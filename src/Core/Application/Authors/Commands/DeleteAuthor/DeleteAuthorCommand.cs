using Application.Abstractions;
using Domain.Shared;

namespace Application.Authors.Commands.DeleteAuthor;

public sealed record DeleteAuthorCommand(Guid AuthorId) : ICommand<Result>;