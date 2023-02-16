using System.Net;
using Application.Books;
using Application.Books.Commands.CreateBook;
using Application.Books.Responses;
using BookMarket.Tests.Extensions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookMarket.Tests.IntegrationTests;

public class BooksApiTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private Guid _existingAuthorId;

    public BooksApiTests()
    {
        _factory = Util.BuildTestServer(_ => { });
        CreateAuthor();
    }

    private void CreateAuthor()
    {
        var authorsRepo = _factory.Services.GetRequiredService<IAuthorRepository>();
        var unitOfWork = _factory.Services.GetRequiredService<IUnitOfWork>();
        var author = new AuthorDto { Name = "Some author" };
        Task.Run(async () =>
        {
            _existingAuthorId = await authorsRepo.Insert(author);
            unitOfWork.Commit();
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task GetBook_BookNotExists_ReturnsError()
    {
        var bookId = Guid.NewGuid();
        var req = ApiRequestBuilder.Book.Get(bookId);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);

        var result = await resp.DeserializeAsync<Result<Error>>();
        Assert.True(result.IsError);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Message);
        Assert.NotNull(result.Data.TraceId);
    }

    [Fact]
    public async Task DeleteBook_BookNotExists_ReturnsError()
    {
        var bookId = Guid.NewGuid();
        var req = ApiRequestBuilder.Book.Delete(bookId);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);

        var result = await resp.DeserializeAsync<Result<Error>>();
        Assert.True(result.IsError);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Message);
        Assert.NotNull(result.Data.TraceId);
    }

    [Fact]
    public async Task CreateBook_AuthorNotExists_ReturnsError()
    {
        var c = new CreateBookRequest(
            "title",
            "desc",
            DateTime.Today,
            321,
            "en",
            Guid.NewGuid());
        var req = ApiRequestBuilder.Book.Create(c);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);

        var deleteResult = await resp.DeserializeAsync<Result>();
        Assert.True(deleteResult.IsError);
    }

    [Fact]
    public async Task CreateDeleteGetBook_CreateBookDeleteBookGetBook_BookDeleted()
    {
        var c = new CreateBookRequest(
            "title",
            "desc",
            DateTime.Today,
            321,
            "en",
            _existingAuthorId);
        var createReq = ApiRequestBuilder.Book.Create(c);
        var createRes = await _factory.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);
        var bookId = (await createRes.DeserializeAsync<Result<Guid>>()).Data; 
        
        var req = ApiRequestBuilder.Book.Delete(bookId);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var deleteResult = await resp.DeserializeAsync<Result>();
        Assert.False(deleteResult.IsError);

        var getReq = ApiRequestBuilder.Book.Get(bookId);
        var getResp = await _factory.SendRequestAsync(getReq);
        Assert.Equal(HttpStatusCode.BadRequest, getResp.StatusCode);
    }
    
    [Theory]
    [InlineData(null, "desc", "2022-02-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", BookValidationErrors.TitleInvalid)]
    [InlineData(Util.SymbolsCount61, "desc", "2022-02-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", BookValidationErrors.TitleInvalid)]
    [InlineData("title", "", "2022-02-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", BookValidationErrors.DescriptionInvalid)]
    [InlineData("title", Util.SymbolsCount513, "2022-02-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", BookValidationErrors.DescriptionInvalid)]
    [InlineData("title", "desc", "0001-01-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", BookValidationErrors.PublishDateInvalid)]
    [InlineData("title", "desc", "2022-02-01", 0, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", BookValidationErrors.PagesCountInvalid)]
    [InlineData("title", "desc", "2022-02-01", 321, "", "3fa85f64-5717-4562-b3fc-2c963f66afa6", BookValidationErrors.LanguageInvalid)]
    [InlineData("title", "desc", "2022-02-01", 321, Util.SymbolsCount21, "3fa85f64-5717-4562-b3fc-2c963f66afa6", BookValidationErrors.LanguageInvalid)]
    [InlineData("title", "desc", "2022-02-01", 321, "en", "00000000-0000-0000-0000-000000000000", BookValidationErrors.AuthorIdInvalid)]
    public async Task CreateBook_ValidationFailure_ReturnsErrorCode(string title, string description, string pDate, short pages, string lang, string author, string validationError)
    {
        DateTime.TryParse(pDate, out var publishDate);
        Guid.TryParse(author, out var authorId);

        var c = new CreateBookRequest(
            title,
            description,
            publishDate,
            pages,
            lang,
            authorId);
        var req = ApiRequestBuilder.Book.Create(c);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);

        var result = await resp.DeserializeAsync<Result<ErrorDetailed<Dictionary<string, string[]>>>>();
        Assert.NotNull(result);
        Assert.True(result.IsError);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Details);
        Assert.NotNull(result.Data.Message);
        Assert.NotNull(result.Data.TraceId);
        Assert.All(result.Data.Details.Values, errorCodes => Assert.Contains(validationError, errorCodes));
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
            _existingAuthorId);
        var createReq = ApiRequestBuilder.Book.Create(c);
        var createRes = await _factory.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);

        var createdBookId = (await createRes.DeserializeAsync<Result<Guid>>()).Data;
        Assert.NotEqual(Guid.Empty, createdBookId);

        var getReq = ApiRequestBuilder.Book.Get(createdBookId);
        var getResp = await _factory.SendRequestAsync(getReq);
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        var b = await getResp.DeserializeAsync<Book>();
        Assert.Equal(c.Title, b.Title);
        Assert.Equal(c.Description, b.Description);
        Assert.Equal(c.PublishDate, b.PublishDate);
        Assert.Equal(c.PagesCount, b.PagesCount);
        Assert.Equal(c.Language, b.Language);
        Assert.Equal(c.AuthorId, b.Author.Id);
        Assert.NotNull(b.Author.Name);
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
            _existingAuthorId);
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
    }

    public void Dispose()
    {
        _factory?.Dispose();
    }
}