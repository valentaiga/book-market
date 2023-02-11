using Application.Abstractions;
using Application.Books.Responses;

namespace Application.Books.Queries.GetAllBooks;

public sealed record GetAllBooksQuery : IQuery<BooksResponse>;