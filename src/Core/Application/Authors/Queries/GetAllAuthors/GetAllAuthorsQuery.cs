using Application.Abstractions;
using Application.Authors.Responses;

namespace Application.Authors.Queries.GetAllAuthors;

public sealed record GetAllAuthorsQuery : IQuery<AuthorsResponse>;