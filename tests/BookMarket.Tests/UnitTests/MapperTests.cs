using Application.Abstractions;
using Application.Authors.Commands.CreateAuthor;
using Application.Authors.Responses;
using Application.Books.Commands.CreateBook;
using Application.Books.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookMarket.Tests.UnitTests;

public class MapperTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IMapper _mapper;

    public MapperTests()
    {
        _factory = Util.BuildTestServer(_ => { });
        _mapper = _factory.Services.GetRequiredService<IMapper>();
    }

    [Fact]
    public void CreateBookRequest_To_CreateBookCommand()
    {
        var r = new CreateBookRequest(
            "title",
            "desc",
            DateTime.Today,
            321,
            "en",
            Guid.NewGuid());

        var c = _mapper.Map<CreateBookRequest, CreateBookCommand>(r);
        Assert.Equal(r.Title, c.Title);
        Assert.Equal(r.Description, c.Description);
        Assert.Equal(r.PublishDate, c.PublishDate);
        Assert.Equal(r.PagesCount, c.PagesCount);
        Assert.Equal(r.Language, c.Language);
        Assert.Equal(r.AuthorId, c.AuthorId);
    }

    [Fact]
    public void CreateBookCommand_To_BookEntity()
    {
        var c = new CreateBookCommand(
            "title",
            "desc",
            DateTime.Today,
            321,
            "en",
            Guid.NewGuid());

        var b = _mapper.Map<CreateBookCommand, BookDto>(c);
        Assert.Equal(c.Title, b.Title);
        Assert.Equal(c.Description, b.Description);
        Assert.Equal(c.PublishDate, b.PublishDate);
        Assert.Equal(c.PagesCount, b.PagesCount);
        Assert.Equal(c.Language, b.Language);
        Assert.Equal(c.AuthorId, b.AuthorId);
    }

    [Fact]
    public void BookEntity_To_BookResponse()
    {
        var b = new BookDto
        {
            Id = Guid.NewGuid(),
            Title = "title",
            Description = "desc",
            PublishDate = DateTime.Today,
            PagesCount = 321,
            Language = "en",
            AuthorId = Guid.NewGuid()
        };

        var r = _mapper.Map<BookDto, Book>(b);
        Assert.Equal(b.Id, r.Id);
        Assert.Equal(b.Title, r.Title);
        Assert.Equal(b.Description, r.Description);
        Assert.Equal(b.PublishDate, r.PublishDate);
        Assert.Equal(b.PagesCount, r.PagesCount);
        Assert.Equal(b.Language, r.Language);
    }

    [Fact]
    public void CreateAuthorRequest_To_CreateAuthorCommand()
    {
        var r = new CreateAuthorRequest("name");
        var c = _mapper.Map<CreateAuthorRequest, CreateAuthorCommand>(r);
        Assert.Equal(r.Name, c.Name);
    }

    [Fact]
    public void CreateAuthorCommand_To_AuthorEntity()
    {
        var c = new CreateAuthorCommand("name");
        var e = _mapper.Map<CreateAuthorCommand, AuthorDto>(c);
        Assert.Equal(c.Name, e.Name);
    }

    [Fact]
    public void AuthorEntity_To_AuthorResponse()
    {
        var e = new AuthorDto
        {
            Id = Guid.NewGuid(),
            Name = "name"
        };
        var r = _mapper.Map<AuthorDto, Author>(e);
        Assert.Equal(e.Name, r.Name);
        Assert.Equal(e.Id, r.Id);
    }

    public void Dispose()
    {
        _factory?.Dispose();
    }
}