
using System.Net;
using System.Text.Json;

namespace TaskManager.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    { _next = next; _logger = logger; }

    public async Task Invoke(HttpContext ctx)
    {
        try { await _next(ctx); }
        catch (UnauthorizedAccessException ex) { await Write(ctx, HttpStatusCode.Unauthorized, ex.Message); }
        catch (InvalidOperationException ex) { await Write(ctx, HttpStatusCode.BadRequest, ex.Message); }
        catch (KeyNotFoundException ex) { await Write(ctx, HttpStatusCode.NotFound, ex.Message); }
        catch (Exception ex) {
            _logger.LogError(ex, "Unhandled");
            await Write(ctx, HttpStatusCode.InternalServerError, "Server error");
        }
    }

    private static Task Write(HttpContext ctx, HttpStatusCode code, string msg)
    {
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = (int)code;
        var body = JsonSerializer.Serialize(new { title = msg, status = (int)code });
        return ctx.Response.WriteAsync(body);
    }
}
