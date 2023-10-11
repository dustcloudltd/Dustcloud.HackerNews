using System.Text;
using Dustcloud.HackerNews.Common.Model;
using Newtonsoft.Json;

namespace Dustcloud.HackerNews.Repository.Services ;

internal class HackerNewsService : IHackerNewsService
{
    private const string HackerNewsUri = "https://hacker-news.firebaseio.com/v0/";
    private const string TopStories = "topstories.json";
    private const string UpdatedStories = "updates.json";
    private const string Item = "item/{0}.json";

    private readonly HttpClient _hackerNewsClient;
    public HackerNewsService()
    {
        _hackerNewsClient = new HttpClient();
        _hackerNewsClient.BaseAddress = new Uri(HackerNewsUri);
    }

    //public async Task<IEnumerable<NewsItem>> GetStoriesAsync(int top)
    //{
        
    //    var builder = new StringBuilder();
    //    builder.Append("[");
    //    var counter = 0;
    //    for(var n = 0; n < 500; n++)
    //    {
    //        var itemResponse = await _hackerNewsClient.GetStringAsync(string.Format(Item, stories[n]));
    //        builder.Append(itemResponse);
    //        if (n < 499)
    //        {
    //            builder.Append(",");
    //        }

    //    }

    //    builder.Append("]");
    //    var newsItems = JsonConvert.DeserializeObject<List<NewsItem>>(builder.ToString());

    //    return newsItems.OrderByDescending(s => s.Score).Take(top);
    //}

    public async Task<IEnumerable<int>> GetAllTopStoriesAsync()
    {
        var response = await _hackerNewsClient.GetStringAsync(TopStories);
        var stories = JsonConvert.DeserializeObject<IEnumerable<int>>(response);

        return stories;

    }

    public async Task<IEnumerable<int>> GetUpdatedStoriesAsync()
    {
        var response = await _hackerNewsClient.GetStringAsync(UpdatedStories);
        var update = JsonConvert.DeserializeObject<Update>(response);
        
        return update?.Items;
    }

    public async Task<NewsItem> GetNewsItemByIdAsync(int id)
    {
        var response = await _hackerNewsClient.GetStringAsync(string.Format(Item, id));
        var item = JsonConvert.DeserializeObject<NewsItem>(response);

        return item;
    }

    public async Task<IEnumerable<NewsItem>> GetNewsItemsByIds(IEnumerable<int> ids)
    {
        var newsItems = new List<NewsItem>();

        foreach (var id in ids)
        {
            var item = await GetNewsItemByIdAsync(id);
            newsItems.Add(item);
        }

        return newsItems;
    }
}

public interface IHackerNewsService
{
    Task<IEnumerable<int>> GetAllTopStoriesAsync();
    Task<IEnumerable<int>> GetUpdatedStoriesAsync();
    Task<NewsItem> GetNewsItemByIdAsync(int id);
    Task<IEnumerable<NewsItem>> GetNewsItemsByIds(IEnumerable<int> ids);
}
