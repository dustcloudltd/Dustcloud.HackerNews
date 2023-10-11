using System.Collections.Concurrent;
using Dustcloud.HackerNews.Common.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Dustcloud.HackerNews.Caching.Cache;

//public class HackerNewsCache : IMemoryCache
//{
//    private ConcurrentDictionary<int, NewsItem> _cache = new();

//    public void Dispose()
//    {
//        _cache.Clear();
//    }

//    public bool TryGetValue(object key, out object items)
//    {
        
//        if (_cache.IsEmpty ||
//            key is not int top)
//        {
//            items = null;
//            return false;
//        }

//        items = _cache.Values
//                      .OrderByDescending(s => s.Score)
//                      .Take(top);
//        return true;
//    }

//    public ICacheEntry CreateEntry(object key)
//    {
//        var item = key as NewsItem;
//        if (!_cache.TryAdd(item.Id, item))
//        {
//            return null;
//        }
        
//    }

//    public void Remove(object o)
//    {
//        if (o is not int key)
//        {
//            return;
//        }
        
//        _cache.TryRemove(key, out _);
//    }

//}

