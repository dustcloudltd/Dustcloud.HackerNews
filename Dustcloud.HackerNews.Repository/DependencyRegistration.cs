using Dustcloud.HackerNews.Repository.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dustcloud.HackerNews.Repository;
public static class DependencyRegistration 
{
    public static IServiceCollection AddHackerNewsRepository(this IServiceCollection services)
    {
        return services.AddSingleton<IHackerNewsService, HackerNewsService>()
            .AddSingleton<IHttpClientProxy, HackerNewsHttpClientProxy>();
    }
}
