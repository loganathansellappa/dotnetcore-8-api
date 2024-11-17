using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
/// <summary>
/// Middleware to handle exceptions occurring during HTTP request processing.
/// </summary>
/// <param name="context">The HttpContext for the current request.</param>
/// <param name="next">The delegate representing the next middleware in the pipeline.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
/// <remarks>
/// This middleware catches specific exceptions and sets the appropriate HTTP status code and message:
/// - NotFoundException: Sets status code to 404 and writes the exception message.
/// - ForbiddenException: Sets status code to 403 and writes the exception message.
/// - General Exception: Sets status code to 500 and writes a generic error message.
/// </remarks>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException exception)
        {
            logger.LogError(exception, exception.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (ForbiddenException exception)
        {
            logger.LogError(exception, exception.Message);
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong, with debugger. Please try again later.");
        }
    }
}