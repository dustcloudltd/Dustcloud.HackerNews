using Dustcloud.HackerNews.Common.Model;
using Newtonsoft.Json;

namespace Dustcloud.HackerNews.Repository.Services ;

internal class HackerNewsService : IHackerNewsService
{
    private const string HackerNewsUri = "https://hacker-news.firebaseio.com/v0/";
    private const string TopStories = "topstories.json";
    private const string Item = "item/{0}.json";

    private readonly HttpClient _hackerNewsClient;
    public HackerNewsService()
    {
        _hackerNewsClient = new HttpClient();
        _hackerNewsClient.BaseAddress = new Uri(HackerNewsUri);
    }

    public async Task<IEnumerable<int>> GetAllTopStoriesAsync()
    {
        var response = await _hackerNewsClient.GetStringAsync(TopStories);
        var stories = JsonConvert.DeserializeObject<IEnumerable<int>>(response);

        return stories;
    }


    private async Task<HackerNewsItem> GetNewsItemByIdAsync(int id)
    {
        var response = await _hackerNewsClient.GetStringAsync(string.Format(Item, id));
        var item = JsonConvert.DeserializeObject<HackerNewsItem>(response);

        return item;
    }

    public async Task<IEnumerable<HackerNewsItem>> GetNewsItemsByIds(IEnumerable<int> ids)
    {
        var newsItems = new List<HackerNewsItem>();

        foreach (var id in ids)
        {
            var item = await GetNewsItemByIdAsync(id);
            newsItems.Add(item);
        }

        return newsItems;
    }
}