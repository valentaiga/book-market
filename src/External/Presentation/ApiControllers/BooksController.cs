using Application.Abstractions;
using Application.Books.Commands.CreateBook;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBookById;
using Application.Books.Responses;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ApiControllers;

/// <summary>
/// Books web api
/// </summary>
[Route("api/books")]
public class BooksController : ApiControllerBase
{
    private readonly IMapper _mapper;

    /// <inheritdoc />
    public BooksController(IMediator mediator, IMapper mapper)
        : base(mediator)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Get a book by Id
    /// </summary>
    /// <param name="bookId">Book Id</param>
    /// <param name="ct"></param>
    /// <returns>Book response</returns>
    [HttpGet("{bookId:guid}")]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid bookId, CancellationToken ct)
    {
        var query = new GetBookByIdQuery(bookId);
        var book = await Mediator.Send(query, ct);
        return Ok(book);
    }

    /// <summary>
    /// Get all books
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>Books response</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BooksResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var query = new GetAllBooksQuery();
        var books = await Mediator.Send(query, ct);
        return Ok(books);
    }

    /// <summary>
    /// Create a book
    /// </summary>
    /// <param name="request">Request body</param>
    /// <returns>Book Id</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateBookRequest request)
    {
        var command = _mapper.Map<CreateBookRequest, CreateBookCommand>(request);
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}