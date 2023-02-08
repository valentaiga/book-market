using Application.Abstractions;

namespace Application.Books.Queries.GetAllBooks;

public sealed record GetAllBooksQuery : IQuery<BooksResponse>;