using Dustcloud.HackerNews.Repository.Model;
using Dustcloud.HackerNews.Repository.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dustcloud.HackerNews.Controllers;

[ApiController]
[Route("[controller]")]
public class HackerNewsController : ControllerBase
{
    private readonly IHackerNewsService _hackerNewsService;
    private readonly ILogger<HackerNewsController> _logger;

    public HackerNewsController(IHackerNewsService hackerNewsService,
        ILogger<HackerNewsController> logger)
    {
        _hackerNewsService = hackerNewsService;
        _logger = logger;
    }

    [HttpGet]//(Name = "GetWeatherForecast")]
    public Task<IEnumerable<NewsItem>> Get(int n)
    {
        return _hackerNewsService.GetStories(n);
    }
}
