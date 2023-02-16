using System.Data;
using Application.Authors.Commands.CreateAuthor;
using Application.Authors.Commands.DeleteAuthor;
using Application.Authors.Queries.GetAllAuthors;
using Application.Authors.Queries.GetAuthorById;
using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBookById;
using BookMarket.Tests.Abstractions;
using Domain.Abstractions;
using Domain.Exceptions.Architecture;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace BookMarket.Tests.UnitTests;

public class DatabaseExceptionsTests : IDisposable
{
    private readonly Guid _someId = Guid.NewGuid();

    private readonly WebApplicationFactory<Program> _factory;
    private readonly IMediator _mediator;
    
    public DatabaseExceptionsTests()
    {
        _factory = Util.BuildTestServer(services =>
        {
            var conMock = new Mock<IDbConnection>();
            conMock.Setup(x => x.Open())
                .Throws(new InternalDbException("Expected"));
            conMock.Setup(x => x.CreateCommand())
                .Throws(new InternalDbException("Expected"));
            
            var factoryMock = new Mock<IDbConnectionFactory>();
            factoryMock.Setup(x => x.GetConnection())
                .Returns(conMock.Object);
            services.RemoveAll<IDbConnectionFactory>();
            services.RemoveAll<PgsqlConnectionFactory>();
            services.AddSingleton<IDbConnectionFactory>(_ => factoryMock.Object);
        });
        _mediator = _factory.Services.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task BooksRepository_DatabaseExceptions()
    {
        var getQ = new GetBookByIdQuery(_someId);
        var getAllQ = new GetAllBooksQuery();
        var deleteC = new DeleteBookCommand(_someId);
        var createC = new CreateBookCommand(
            "DatabaseNotAvailableTitle",
            "desc",
            DateTime.Today,
            12,
            "en",
            _someId);
        
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(getQ));
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(getAllQ));
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(createC));
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(deleteC));
    }

    [Fact]
    public async Task AuthorsRepository_DatabaseExceptions()
    {
        var getQ = new GetAuthorByIdQuery(_someId);
        var getAllQ = new GetAllAuthorsQuery();
        var deleteC = new DeleteAuthorCommand(_someId);
        var createC = new CreateAuthorCommand("name");
        
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(getQ));
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(getAllQ));
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(createC));
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(deleteC));
    }

    public void Dispose()
    {
        _factory?.Dispose();
    }
}