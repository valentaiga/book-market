using System.Net;
using Application.Authors;
using Application.Authors.Commands.CreateAuthor;
using Application.Authors.Responses;
using BookMarket.Tests.Extensions;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BookMarket.Tests.IntegrationTests;

public class AuthorsApiTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthorsApiTests()
    {
        _factory = Util.BuildTestServer(_ => { });
    }

    [Fact]
    public async Task GetAuthor_AuthorNotExists_ReturnsError()
    {
        var id = Guid.NewGuid();
        var req = ApiRequestBuilder.Author.Get(id);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);

        var result = await resp.DeserializeAsync<Result<Error>>();
        Assert.True(result.IsError);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Message);
        Assert.NotNull(result.Data.TraceId);
    }

    [Fact]
    public async Task DeleteAuthor_AuthorNotExists_ReturnsError()
    {
        var id = Guid.NewGuid();
        var req = ApiRequestBuilder.Author.Delete(id);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);

        var result = await resp.DeserializeAsync<Result<Error>>();
        Assert.True(result.IsError);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Message);
        Assert.NotNull(result.Data.TraceId);
    }

    [Fact]
    public async Task CreateDeleteGetAuthor_CreateAuthorDeleteAuthorGetAuthor_AuthorDeleted()
    {
        var c = new CreateAuthorRequest("name");
        var createReq = ApiRequestBuilder.Author.Create(c);
        var createRes = await _factory.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);
        var authorId = (await createRes.DeserializeAsync<Result<Guid>>()).Data; 
        
        var req = ApiRequestBuilder.Author.Delete(authorId);
        var resp = await _factory.SendRequestAsync(req);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var deleteResult = await resp.DeserializeAsync<Result>();
        Assert.False(deleteResult.IsError);

        var getReq = ApiRequestBuilder.Author.Get(authorId);
        var getResp = await _factory.SendRequestAsync(getReq);
        Assert.Equal(HttpStatusCode.BadRequest, getResp.StatusCode);
    }

    [Theory]
    [InlineData("", AuthorValidationErrors.NameInvalid)]
    [InlineData(Util.SymbolsCount61, AuthorValidationErrors.NameInvalid)]
    public async Task CreateAuthor_ValidationFailure_ReturnsErrorCode(string name, string validationError)
    {
        var c = new CreateAuthorRequest(name);
        var req = ApiRequestBuilder.Author.Create(c);
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
    public async Task CreateGetAuthor_CreateAuthorGetAuthor_ReturnsSameAuthor()
    {
        var c = new CreateAuthorRequest("name");
        var createReq = ApiRequestBuilder.Author.Create(c);
        var createRes = await _factory.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);

        var authorId = (await createRes.DeserializeAsync<Result<Guid>>()).Data;
        Assert.NotEqual(Guid.Empty, authorId);

        var getReq = ApiRequestBuilder.Author.Get(authorId);
        var getResp = await _factory.SendRequestAsync(getReq);
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        var b = await getResp.DeserializeAsync<Author>();
        Assert.Equal(authorId, b.Id);
        Assert.Equal(c.Name, b.Name);
    }

    [Fact]
    public async Task CreateGetAllBooks_CreateBookGetAllBooks_ReturnsSameBook()
    {
        var c = new CreateAuthorRequest("Name");
        var createReq = ApiRequestBuilder.Author.Create(c);
        var createRes = await _factory.SendRequestAsync(createReq);
        Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);

        var authorId = (await createRes.DeserializeAsync<Result<Guid>>()).Data;
        Assert.NotEqual(Guid.Empty, authorId);

        var getAllReq = ApiRequestBuilder.Author.GetAll();
        var getAllResp = await _factory.SendRequestAsync(getAllReq);
        Assert.Equal(HttpStatusCode.OK, getAllResp.StatusCode);
        var getAllResult = await getAllResp.DeserializeAsync<AuthorsResponse>();
        Assert.Contains(getAllResult.Authors, book => book.Id == authorId);

        var b = getAllResult.Authors.FirstOrDefault(x => x.Id == authorId);
        Assert.NotNull(b);
        Assert.Equal(authorId, b.Id);
        Assert.Equal(c.Name, b.Name);
    }

    public void Dispose()
    {
        _factory?.Dispose();
    }
}