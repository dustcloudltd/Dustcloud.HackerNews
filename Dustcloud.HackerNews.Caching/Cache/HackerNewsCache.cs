using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dustcloud.HackerNews.Repository.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Dustcloud.HackerNews.Caching.Cache ;
public class HackerNewsCache : IMemoryCache
{
    private Dictionary<int, NewsItem> _cache = new();
    public void Dispose()
    {
        
    }

    public bool TryGetValue(object key, out object value)
    {
        
    }

    public ICacheEntry CreateEntry(object key)
    {
    }

    public void Remove(object key)
    {
    }
}
