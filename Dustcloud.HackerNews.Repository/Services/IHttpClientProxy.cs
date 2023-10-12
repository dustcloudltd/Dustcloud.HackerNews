namespace Dustcloud.HackerNews.Repository.Services;

public interface IHttpClientProxy
{
    Task<string> GetStringAsync(string url);
    Uri BaseAddress { get; set; }
}