using System.Net;
using LinkShortener.Application.Common.Exceptions;

namespace LinkShortener.Api.Middleware;

public class ExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (OldRefreshTokenException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
        catch (Exception e)
        {
            context.Response.Clear();
            context.Response.ContentType = "text";
            await context.Response.WriteAsync(e.Message);
        }
    }
}