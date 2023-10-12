namespace Dustcloud.HackerNews.Middleware ;

public class HackerNewsMiddleware
{
    private readonly RequestDelegate _next;

    public HackerNewsMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        //if (context.Request.Headers) -- check for authentication token?
        await _next(context);
    }
}
