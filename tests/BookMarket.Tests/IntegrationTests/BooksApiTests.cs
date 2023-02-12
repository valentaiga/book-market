using System.Net;
using Application.Books.Commands.CreateBook;
using Application.Books.Responses;
using BookMarket.Tests.Extensions;
using Domain.Shared;
using Xunit;

namespace BookMarket.Tests.IntegrationTests;

[Collection(IntegrationCollectionDefinition.DefinitionName)]
public class BooksApiTests
{
    private readonly IntegrationTestsFixture _fixture;

    public BooksApiTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_BookNotExists_400BadRequest()
    {
        var bookId = Guid.NewGuid();
        var req = ApiRequestBuilder.Book.Get(bookId);
        var resp = await _fixture.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        
        var result = await resp.DeserializeAsync<Result>();
        Assert.True(result.IsError);
        Assert.NotNull(result.Error?.Message);
        Assert.NotNull(result.Error?.TraceId);
    }

    [Fact]
    public async Task CreateGet_CreateBookGetBook_ReturnsSameBook()
    {
        var c = new CreateBookRequest(
            "title",
            "desc",
            DateTime.Today,
            321,
            "en",
            Guid.NewGuid());
        var createReq = ApiRequestBuilder.Book.Create(c);
        var createRes = await _fixture.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);

        var createdBookId = (await createRes.DeserializeAsync<Result<Guid>>()).Data;
        Assert.NotEqual(Guid.Empty, createdBookId);

        var getReq = ApiRequestBuilder.Book.Get(createdBookId);
        var getResp = await _fixture.SendRequestAsync(getReq);
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
    public async Task CreateGetAll_CreateBookGetAllBooks_ReturnsSameBook()
    {
        var c = new CreateBookRequest(
            "title",
            "desc",
            DateTime.Today,
            321,
            "en",
            Guid.NewGuid());
        var createReq = ApiRequestBuilder.Book.Create(c);
        var createRes = await _fixture.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);

        var createdBookId = (await createRes.DeserializeAsync<Result<Guid>>()).Data;
        Assert.NotEqual(Guid.Empty, createdBookId);

        var getAllReq = ApiRequestBuilder.Book.GetAll();
        var getAllResp = await _fixture.SendRequestAsync(getAllReq);
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
}