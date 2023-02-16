using Application.Abstractions;
using Application.Authors.Commands.CreateAuthor;
using Application.Authors.Commands.DeleteAuthor;
using Application.Authors.Queries.GetAllAuthors;
using Application.Authors.Queries.GetAuthorById;
using Application.Authors.Responses;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ApiControllers;

/// <summary>
/// Authors web api
/// </summary>
[Route("api/authors")]
public class AuthorsController : ApiControllerBase
{
    private readonly IMapper _mapper;

    /// <inheritdoc />
    public AuthorsController(IMediator mediator, IMapper mapper)
        : base(mediator)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Get an author by Id
    /// </summary>
    /// <param name="authorId">author Id</param>
    /// <param name="ct"></param>
    /// <returns>Author response</returns>
    [HttpGet("{authorId:guid}")]
    [ProducesResponseType(typeof(Author), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid authorId, CancellationToken ct)
    {
        var query = new GetAuthorByIdQuery(authorId);
        var author = await Mediator.Send(query, ct);
        return Ok(author);
    }

    /// <summary>
    /// Get all authors
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>Authors response</returns>
    [HttpGet]
    [ProducesResponseType(typeof(AuthorsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var query = new GetAllAuthorsQuery();
        var authors = await Mediator.Send(query, ct);
        return Ok(authors);
    }

    /// <summary>
    /// Create an author
    /// </summary>
    /// <param name="request">Request body</param>
    /// <returns>author Id</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateAuthorRequest request)
    {
        var command = _mapper.Map<CreateAuthorRequest, CreateAuthorCommand>(request);
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    /// <summary>
    /// Delete an author
    /// </summary>
    /// <param name="authorId">Author id</param>
    /// <returns>Operation success</returns>
    [HttpDelete("{authorId:guid}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(Guid authorId)
    {
        var command = new DeleteAuthorCommand(authorId);
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}