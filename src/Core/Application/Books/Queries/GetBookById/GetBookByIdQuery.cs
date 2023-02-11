using Application.Abstractions;
using Application.Books.Responses;

namespace Application.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(Guid BookId) : IQuery<BookResponse>;