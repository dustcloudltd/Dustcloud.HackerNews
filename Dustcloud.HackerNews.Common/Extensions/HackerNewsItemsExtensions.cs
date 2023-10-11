using Dustcloud.HackerNews.Common.Model;

namespace Dustcloud.HackerNews.Common.Extensions;

public static class HackerNewsItemsExtensions
{
    public static List<DustcloudNewsItem> MapToDustcloudNewsItems(this IEnumerable<HackerNewsItem> hackerNewsStories)
    {
        var dItems = new List<DustcloudNewsItem>();

        foreach (var h in hackerNewsStories)
        {
            dItems.Add(new()
            {
                Id = h.Id,
                CommentCount = h.Descendants,
                PostedBy = h.By,
                Score = h.Score,
                Time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(h.Time),
                Title = h.Title,
                Uri = h.Url
            });
        }

        return dItems;
    }
}
