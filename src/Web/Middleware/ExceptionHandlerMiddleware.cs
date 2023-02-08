using System.Text.Json;
using Application.Exceptions;
using Domain.Exceptions.Base;
using Domain.Shared;
using static Domain.Shared.Result;

namespace Web.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = context.TraceIdentifier;
        context.Response.ContentType = "application/json";
        switch (exception)
        {
            case ValidationException ex:
            {
                var error = Failure(ex.Message, traceId, ex.Errors);
                await WriteResponse(context, error, StatusCodes.Status400BadRequest);
                break;
            }
            case InternalErrorException ex:
            {
                var error = Failure("Internal Service Error", traceId);
                await WriteResponse(context, error, StatusCodes.Status500InternalServerError);
                break;
            }
            default:
            {
                _logger.LogError(exception, exception.Message);
                var error = Failure("Service Unavailable", traceId);
                await WriteResponse(context, error, StatusCodes.Status503ServiceUnavailable);
                break;
            }
        }
    }

    private static async Task WriteResponse(HttpContext context, Result error, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        var bytes = JsonSerializer.SerializeToUtf8Bytes(error);
        await context.Response.BodyWriter.WriteAsync(bytes);
    }
}