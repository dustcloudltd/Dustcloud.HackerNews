using System.Text;
using Dustcloud.HackerNews.Repository.Model;
using Newtonsoft.Json;

namespace Dustcloud.HackerNews.Repository.Services ;

internal class HackerNewsService : IHackerNewsService
{
    private const string HackerNewsUri = "https://hacker-news.firebaseio.com/v0/";
    private const string TopStories = "topstories.json";
    private const string Item = "item/{0}.json";

    private HttpClient _hackerNewsClient;
    public HackerNewsService()
    {
        _hackerNewsClient = new HttpClient();
        _hackerNewsClient.BaseAddress = new Uri(HackerNewsUri);
        //v0/topstories.json"
    }

    public async Task<IEnumerable<NewsItem>> GetStories(int top)
    {
        var response = await _hackerNewsClient.GetStringAsync(TopStories);
        var stories = JsonConvert.DeserializeObject<List<int>>(response);

        var builder = new StringBuilder();
        builder.Append("[");
        var counter = 0;
        for(var n = 0; n < 500; n++)
        {
            var itemResponse = await _hackerNewsClient.GetStringAsync(string.Format(Item, stories[n]));
            builder.Append(itemResponse);
            if (n < 499)
            {
                builder.Append(",");
            }

        }

        builder.Append("]");
        var newsItems = JsonConvert.DeserializeObject<List<NewsItem>>(builder.ToString());

        return newsItems.OrderByDescending(s => s.Score).Take(top);
    }
}

public interface IHackerNewsService
{
    Task<IEnumerable<NewsItem>> GetStories(int s);

}
