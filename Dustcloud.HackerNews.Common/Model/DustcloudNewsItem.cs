using System.Text.Json.Serialization;

namespace Dustcloud.HackerNews.Common.Model;

public class DustcloudNewsItem
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    [JsonPropertyName("postedBy")]
    public string PostedBy { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("commentCount")]
    public int CommentCount { get; set; }
}