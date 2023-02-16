using Application.Abstractions.Cache;
using Application.Authors.Commands.DeleteAuthor;
using Application.Authors.Queries.GetAllAuthors;
using Application.Authors.Queries.GetAuthorById;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BookMarket.Tests.UnitTests;

public class CacheTests
{
    private readonly WebApplicationFactory<Program> _factory;

    private readonly BookDto _book = new BookDto { Id = Guid.NewGuid(), Title = "Some title", Description = "Some desc", Language = "s lang", PagesCount = 32, PublishDate = DateTime.Today, AuthorId = Guid.NewGuid()};
    private readonly IMediator _mediator;
    private short _getByIdChangedValue = 3;
    private short _getAllBooksChangedValue = 3;

    public CacheTests()
    {
        _factory = Util.BuildTestServer(services =>
        {
            var authorMock = new Mock<IAuthorRepository>();
            authorMock.Setup(x => x.Delete(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            authorMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns<Guid, CancellationToken>((id, _) =>
                    Task.FromResult(new AuthorDto { Id = id, Name = $"{++_getByIdChangedValue}" }));
            authorMock.Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new[] { new AuthorDto { Id = Guid.NewGuid(), Name = $"{++_getAllBooksChangedValue}" } });

            var booksMock = new Mock<IBookRepository>();
            booksMock.Setup(x => x.GetByAuthor(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { _book });
            
            services.AddScoped<IBookRepository>(src => booksMock.Object);
            services.AddScoped<IAuthorRepository>(src => new AuthorCacheDecorator(authorMock.Object, src.GetRequiredService<ICacheProvider>()));
        });

        _mediator = _factory.Services.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task GetAuthorById_GetAuthorByIdInsertAuthor_CacheCleaned()
    {
        var q = new GetAuthorByIdQuery(Guid.NewGuid());
        var getResult1 = await _mediator.Send(q, CancellationToken.None);
        
        var c = new DeleteAuthorCommand(q.AuthorId);
        await _mediator.Send(c);
        var getResult2 = await  _mediator.Send(q, CancellationToken.None);
        
        Assert.Equal(q.AuthorId, getResult1.Id);
        Assert.Equal(q.AuthorId, getResult2.Id);
        Assert.NotEqual(getResult1.Name, getResult2.Name); // name equals repo calls count
    }

    [Fact]
    public async Task GetAuthorById_GetAuthorById_RepoCalledOnce()
    {
        var q = new GetAuthorByIdQuery(Guid.NewGuid());
        var getResult1 = await _mediator.Send(q, CancellationToken.None);
        var getResult2 = await  _mediator.Send(q, CancellationToken.None);
        
        Assert.Equal(q.AuthorId, getResult1.Id);
        Assert.Equal(q.AuthorId, getResult2.Id);
        Assert.Equal(getResult1.Name, getResult2.Name); // name equals repo calls count
    }
    
    [Fact]
    public async Task GetAllAuthorsCached_GetAllAuthors_AllFieldsAreFilled()
    {
        var q = new GetAuthorByIdQuery(Guid.NewGuid());
        
        await _mediator.Send(q, CancellationToken.None); // cache setup call
        var getResult = await _mediator.Send(q, CancellationToken.None);

        Assert.Equal(q.AuthorId, getResult.Id);
        Assert.NotEqual("0", getResult.Name);
        Assert.All(getResult.Books, book =>
        {
            Assert.Equal(_book.Title, book.Title);
            Assert.Equal(_book.Description, book.Description);
            Assert.Equal(_book.PublishDate, book.PublishDate);
            Assert.Equal(_book.PagesCount, book.PagesCount);
            Assert.Equal(_book.Language, book.Language);
        });
    }

    [Fact]
    public async Task GetAllAuthors_GetAllAuthors_RepoCalledOnce()
    {
        var q = new GetAllAuthorsQuery();
        var getResult1 = await _mediator.Send(q, CancellationToken.None);
        var getResult2 = await _mediator.Send(q, CancellationToken.None);

        Assert.Equal(getResult1.Authors.Length, getResult2.Authors.Length);
        for (var i = 0; i < getResult1.Authors.Length; i++)
        {
            Assert.Equal(getResult1.Authors[i].Id, getResult2.Authors[i].Id);
            Assert.Equal(getResult1.Authors[i].Name, getResult2.Authors[i].Name);
        }
    }
}