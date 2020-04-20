using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TezYonetimSistemi.UI.Helpers
{
    public class CacheProvider
    {
        private readonly static System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;

        public static void CacheEkle(string key, object value, int expireAsMinute)
        {
            if (_cache.Any(x => x.Key == key))
            {
                _cache.Remove(key);
            }
            _cache.Add(key, value, DateTimeOffset.Now.AddMinutes(expireAsMinute));
        }

        public static object CachedenOku(string key) => _cache.Get(key);

        public static void CacheSil(string key)
        {
            if (_cache.Any(x => x.Key == key))
            {
                _cache.Remove(key);
            }
        }
    }
}