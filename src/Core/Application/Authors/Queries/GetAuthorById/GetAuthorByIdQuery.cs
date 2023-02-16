using Application.Abstractions;
using Application.Authors.Responses;

namespace Application.Authors.Queries.GetAuthorById;

public sealed record GetAuthorByIdQuery(Guid AuthorId) : IQuery<Author>;