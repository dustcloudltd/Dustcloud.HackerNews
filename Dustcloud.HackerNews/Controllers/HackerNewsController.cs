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
    private readonly ILogger<HackerNewsController> _logger;
    private const string NewsStoryCacheKey = "NewsStory";
    private const string UpdatedCacheKey = "Updated";

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
            if (_memoryCache.TryGetValue(NewsStoryCacheKey, out IEnumerable<NewsItem> items))
            {
                var topItems = items.OrderByDescending(s => s.Score).Take(top);

                var isUpdatedCached = _memoryCache.TryGetValue(UpdatedCacheKey, out IEnumerable<NewsItem> updatedItems);
                if (!isUpdatedCached)
                {
                    await SetUpdatedItemsFromCacheAsync();
                }
                else
                {
                    var isUpdatedCacheAndApiSame = await CompareCacheWithApiAsync(updatedItems.ToList());
                    if (isUpdatedCacheAndApiSame)
                    {
                        return Ok(topItems);
                    }
                    else
                    {
                        await SetUpdatedItemsFromCacheAsync();
                    }
                }

            }
            else
            {
                await SetTopItemsCacheAsync();
                _memoryCache.TryGetValue(NewsStoryCacheKey, out items);
            }

            return Ok(items.OrderByDescending(s => s.Score).Take(top));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //True if they're the same
    private async Task<bool> CompareCacheWithApiAsync(IEnumerable<NewsItem> cachedUpdatedItems)
    {
        var updatedIds = (await _hackerNewsService.GetUpdatedStoriesAsync()).ToList();
        var cachedUpdatedIds = cachedUpdatedItems.Select(s => s.Id).ToList();
        return updatedIds.Count() == cachedUpdatedIds.Count() && !updatedIds.Except(cachedUpdatedIds).Any();
    }


    private async Task SetTopItemsCacheAsync()
    {
        _logger.Log(LogLevel.Information, "Setting top items cache");
        var allStories = await _hackerNewsService.GetAllTopStoriesAsync();
        
        // This is the long-un
        var storiesWithBodies = await _hackerNewsService.GetNewsItemsByIds(allStories);

        SetCache(NewsStoryCacheKey, storiesWithBodies);

    }

    private async Task SetUpdatedItemsFromCacheAsync()
    {
        _logger.Log(LogLevel.Information, "Setting updated items cache");
        var updatedIds = await _hackerNewsService.GetUpdatedStoriesAsync();
        var updatedItems = await _hackerNewsService.GetNewsItemsByIds(updatedIds);

        SetCache(UpdatedCacheKey, updatedItems, 60);
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
