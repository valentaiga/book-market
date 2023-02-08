using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ApiControllers;

#pragma warning disable CS1591

/// <inheritdoc />
[ProducesResponseType(typeof(ErrorDetailed<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(Error), StatusCodes.Status503ServiceUnavailable)]
[Route("api")]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly IMediator Mediator;

    protected ApiControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }
}
#pragma warning restore CS1591
