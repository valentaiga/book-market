using Application.Abstractions;

namespace Application.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(Guid BookId) : IQuery<BookResponse>;