using Dustcloud.HackerNews.Common.Extensions;
using Dustcloud.HackerNews.Common.Model;
using Dustcloud.HackerNews.Repository.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Dustcloud.HackerNews.Controllers;

[ApiController]
[Route("[controller]")]
public class HackerNewsController : ControllerBase
{
    private readonly IHackerNewsService _hackerNewsService;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;
    private const string NewsStoryCacheKey = "NewsStory";

    public HackerNewsController(IHackerNewsService hackerNewsService,
        IMemoryCache memoryCache,
        ILogger<HackerNewsController> logger)
    {
        _hackerNewsService = hackerNewsService;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    [HttpGet(Name = "GetHackerNews")]
    public async Task<IActionResult> GetHackerNews(int top)
    {
        try
        {
            if (!_memoryCache.TryGetValue(NewsStoryCacheKey, out List<DustcloudNewsItem> items) ||
                !await CompareCacheWithApiAsync(items.OrderByDescending(s => s.Score).Take(top)))
            {
                await SetTopItemsCacheAsync();
                _memoryCache.TryGetValue(NewsStoryCacheKey, out items);
            }
            
            var topItems = items.OrderByDescending(s => s.Score).Take(top);
            return Ok(topItems);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //True if they're the same
    private async Task<bool> CompareCacheWithApiAsync(IEnumerable<DustcloudNewsItem> cachedItems)
    {
        var topStoriesIds = (await _hackerNewsService.GetAllTopStoriesAsync()).ToList();
        var cachedItemIds = cachedItems.Select(s => s.Id).ToList();
        return topStoriesIds.FindAll(s => cachedItemIds.Contains(s)).Count == cachedItemIds.Count;
    }


    private async Task SetTopItemsCacheAsync()
    {
        _logger.Log(LogLevel.Information, "Setting top items cache");
        var allStories = await _hackerNewsService.GetAllTopStoriesAsync();
        
        // This is the long-un
        var hackerNewsStories = await _hackerNewsService.GetNewsItemsByIds(allStories);
        var dustcloudStories = hackerNewsStories.MapToDustcloudNewsItems();
        SetCache(NewsStoryCacheKey, dustcloudStories);
    }

   

    private void SetCache(string cacheKey, object body, int slidingExpiration = 300)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpiration))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
            .SetPriority(CacheItemPriority.Normal)
            .SetSize(500);

        _memoryCache.Set(cacheKey, body, cacheEntryOptions);
    }
}
