using Application.Abstractions;
using Domain.Shared;

namespace Application.Authors.Commands.CreateAuthor;

public sealed record CreateAuthorCommand(
    string Name) : ICommand<Result<Guid>>;