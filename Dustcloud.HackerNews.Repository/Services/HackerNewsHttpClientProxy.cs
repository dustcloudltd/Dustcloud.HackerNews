namespace Dustcloud.HackerNews.Repository.Services;

internal class HackerNewsHttpClientProxy : IHttpClientProxy
{
    private const string HackerNewsUri = "https://hacker-news.firebaseio.com/v0/";
    private readonly HttpClient _httpClient = new();

    public HackerNewsHttpClientProxy()
    {
        _httpClient.BaseAddress = new Uri(HackerNewsUri);
    }
    public Uri BaseAddress { get; set; }

    public Task<string> GetStringAsync(string url)
    {
        return _httpClient.GetStringAsync(url);
    } 
}