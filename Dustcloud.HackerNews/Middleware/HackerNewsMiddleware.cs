namespace Dustcloud.HackerNews.Middleware ;

public class HackerNewsMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        await next(context);
    }
}
