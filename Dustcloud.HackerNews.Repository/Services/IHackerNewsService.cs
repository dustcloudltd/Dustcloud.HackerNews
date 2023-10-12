using Dustcloud.HackerNews.Common.Model;

namespace Dustcloud.HackerNews.Repository.Services;

public interface IHackerNewsService
{
    Task<IEnumerable<int>> GetAllTopStoryIdsAsync();
    Task<IEnumerable<HackerNewsItem>> GetNewsItemsByIds(IEnumerable<int> ids);
}