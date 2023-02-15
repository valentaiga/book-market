using System.Data;
using Application.Books.Commands.CreateBook;
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

public class BooksExceptionsTests : IDisposable
{
    private readonly Guid _notExistingId = Guid.NewGuid();
    private readonly Guid _databaseNotAvailableId = Guid.NewGuid();
    private const string DatabaseNotAvailableTitle = "unavailable";
    private readonly Guid _existingAuthorId = Guid.NewGuid();

    private readonly WebApplicationFactory<Program> _factory;
    private readonly IMediator _mediator;
    
    public BooksExceptionsTests()
    {
        _factory = Util.BuildTestServer(services =>
        {
            var conMock = new Mock<IDbConnection>();
            conMock.Setup(x => x.Open())
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
    public async Task GetBookById_DatabaseNotAvailable_DatabaseException()
    {
        var query = new GetBookByIdQuery(_databaseNotAvailableId);
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(query));
    }

    [Fact]
    public async Task GetAllBooks_DatabaseNotAvailable_DatabaseException()
    {
        var query = new GetAllBooksQuery();
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(query));
    }

    [Fact]
    public async Task InsertBook_DatabaseNotAvailable_DatabaseException()
    {
        var query = new CreateBookCommand(
            DatabaseNotAvailableTitle,
            string.Empty,
            DateTime.Today,
            1,
            string.Empty,
            _existingAuthorId);
        await Assert.ThrowsAsync<DatabaseException>(() => _mediator.Send(query));
    }

    public void Dispose()
    {
        _factory?.Dispose();
    }
}