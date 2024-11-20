using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
            var stopwatch = Stopwatch.StartNew();
            await next.Invoke(context);
            stopwatch.Stop();
            if (_MoreThanFourSeconds(stopwatch.ElapsedMilliseconds))
            {
                logger.LogInformation("Request [{Verb}] at {Path} took {Time} ms", context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);    
            }
            
    }

    private bool _MoreThanFourSeconds(double milliSeconds)
    {
        return (milliSeconds > 4000);
    }
}