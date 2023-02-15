using System.Net;
using Application.Books.Commands.CreateBook;
using Application.Books.Responses;
using BookMarket.Tests.Extensions;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BookMarket.Tests.IntegrationTests;

public class BooksApiTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;

    public BooksApiTests()
    {
        _factory = Util.BuildTestServer(_ => { });
    }

    [Fact]
    public async Task GetBook_BookNotExists_400BadRequest()
    {
        var bookId = Guid.NewGuid();
        var req = ApiRequestBuilder.Book.Get(bookId);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);

        var result = await resp.DeserializeAsync<Result>();
        Assert.True(result.IsError);
        Assert.NotNull(result.Error?.Message);
        Assert.NotNull(result.Error?.TraceId);
    }

    [Fact]
    public async Task CreateGetBook_CreateBookGetBook_ReturnsSameBook()
    {
        var c = new CreateBookRequest(
            "title",
            "desc",
            DateTime.Today,
            321,
            "en",
            Guid.NewGuid());
        var createReq = ApiRequestBuilder.Book.Create(c);
        var createRes = await _factory.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);

        var createdBookId = (await createRes.DeserializeAsync<Result<Guid>>()).Data;
        Assert.NotEqual(Guid.Empty, createdBookId);

        var getReq = ApiRequestBuilder.Book.Get(createdBookId);
        var getResp = await _factory.SendRequestAsync(getReq);
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        var b = await getResp.DeserializeAsync<BookResponse>();
        Assert.Equal(c.Title, b.Title);
        Assert.Equal(c.Description, b.Description);
        Assert.Equal(c.PublishDate, b.PublishDate);
        Assert.Equal(c.PagesCount, b.PagesCount);
        Assert.Equal(c.Language, b.Language);
        Assert.Equal(c.AuthorId, b.AuthorId);
    }

    [Fact]
    public async Task CreateGetAllBooks_CreateBookGetAllBooks_ReturnsSameBook()
    {
        var c = new CreateBookRequest(
            "title",
            "desc",
            DateTime.Today,
            321,
            "en",
            Guid.NewGuid());
        var createReq = ApiRequestBuilder.Book.Create(c);
        var createRes = await _factory.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);

        var createdBookId = (await createRes.DeserializeAsync<Result<Guid>>()).Data;
        Assert.NotEqual(Guid.Empty, createdBookId);

        var getAllReq = ApiRequestBuilder.Book.GetAll();
        var getAllResp = await _factory.SendRequestAsync(getAllReq);
        Assert.Equal(HttpStatusCode.OK, getAllResp.StatusCode);
        var getAllResult = await getAllResp.DeserializeAsync<BooksResponse>();
        Assert.Contains(getAllResult.Books, book => book.Id == createdBookId);

        var b = getAllResult.Books.First(x => x.Id == createdBookId);
        Assert.Equal(c.Title, b.Title);
        Assert.Equal(c.Description, b.Description);
        Assert.Equal(c.PublishDate, b.PublishDate);
        Assert.Equal(c.PagesCount, b.PagesCount);
        Assert.Equal(c.Language, b.Language);
        Assert.Equal(c.AuthorId, b.AuthorId);
    }

    public void Dispose()
    {
        _factory?.Dispose();
    }
}