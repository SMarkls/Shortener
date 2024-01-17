using System.Net;
using LinkShortener.Application.Common.Exceptions.Common;

namespace LinkShortener.Api.Middleware;

public class ExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ApiException e)
        {
            context.Response.Clear();
            context.Response.ContentType = "text";
            context.Response.StatusCode = e.Code;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            context.Response.Clear();
            context.Response.ContentType = "text";
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await context.Response.WriteAsync(e.Message);
        }
    }
}