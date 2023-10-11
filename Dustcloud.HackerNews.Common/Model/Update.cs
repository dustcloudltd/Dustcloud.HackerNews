using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Dustcloud.HackerNews.Common.Model
{
    public class Update
    {
        [JsonPropertyName("items")]
        public int[] Items { get; set; }
    }
}
