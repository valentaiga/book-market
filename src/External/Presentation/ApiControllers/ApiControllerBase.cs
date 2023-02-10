using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ApiControllers;

/// <summary>
/// Abstract API controller class
/// </summary>
/// <response code="400">Request validation fail</response>
/// <response code="500">Internal server error (and we know about it)</response>
/// <response code="503">Service unavailable</response>
[ProducesResponseType(typeof(ErrorDetailed<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
[Route("api")]
public abstract class ApiControllerBase : ControllerBase
{
    /// <inheritdoc cref="IMediator" />
    protected readonly IMediator Mediator;

    /// <summary>
    /// ctor
    /// </summary>
    protected ApiControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }
}