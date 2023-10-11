namespace Dustcloud.HackerNews.Middleware ;

public class HackerNewsMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //check the header for token and authorization

        await next(context);
    }
}
