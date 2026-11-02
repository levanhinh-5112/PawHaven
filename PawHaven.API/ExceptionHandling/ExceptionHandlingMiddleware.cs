using Microsoft.AspNetCore.Mvc;
using PawHaven.Application.Exceptions;
using System.Net;

namespace PawHaven.API.ExceptionHandling;

/// <summary>
/// Middleware that catches application exceptions and converts them to standardized ProblemDetails responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            if (exception is ValidationException or NotFoundException or ConflictException)
            {
                _logger.LogWarning(exception, "Handled application exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
            }
            else
            {
                _logger.LogError(exception, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
            }

            await HandleExceptionAsync(context, exception);
        }
    }

    /// <summary>
    /// Converts application exceptions to ProblemDetails responses with appropriate HTTP status codes.
    /// </summary>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var response = new ProblemDetails();

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Status = StatusCodes.Status400BadRequest;
                response.Title = "One or more validation errors occurred.";
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

                // Add detailed error information for each field
                var errors = new Dictionary<string, string[]>();
                foreach (var error in validationException.Errors)
                {
                    errors[error.Key] = error.Value;
                }
                response.Extensions["errors"] = errors;
                break;

            case NotFoundException notFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.Status = StatusCodes.Status404NotFound;
                response.Title = "The specified resource was not found.";
                response.Detail = notFoundException.Message;
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                break;

            case ConflictException conflictException:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                response.Status = StatusCodes.Status409Conflict;
                response.Title = "A conflict occurred with the request.";
                response.Detail = conflictException.Message;
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
                break;

            default:
                // For unhandled exceptions, return a generic 500 error
                // (Catch-all 500 handling deferred to PAW-33 as per requirements)
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Status = StatusCodes.Status500InternalServerError;
                response.Title = "An internal server error occurred.";
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                break;
        }

        return context.Response.WriteAsJsonAsync(response, options: null, contentType: "application/problem+json");
    }
}
