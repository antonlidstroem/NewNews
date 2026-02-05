using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using NewNews.DAL.Models;

namespace NewNews.MAUI.Services
{
    public class NewsCacheService : INewsCacheService
    {
        private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(15);
        private readonly int _maxCacheSize = 100; // Max antal cachade sidor

        private class CacheEntry
        {
            public List<News> News { get; set; } = new();
            public DateTime ExpiresAt { get; set; }
        }

        public bool TryGet(string cacheKey, out List<News> news)
        {
            if (_cache.TryGetValue(cacheKey, out var entry))
            {
                if (entry.ExpiresAt > DateTime.UtcNow)
                {
                    news = entry.News;
                    System.Diagnostics.Debug.WriteLine($"Cache HIT: {cacheKey} ({news.Count} articles)");
                    return true;
                }
                else
                {
                    // Ta bort utgången cache
                    _cache.TryRemove(cacheKey, out _);
                    System.Diagnostics.Debug.WriteLine($"Cache EXPIRED: {cacheKey}");
                }
            }

            news = new List<News>();
            System.Diagnostics.Debug.WriteLine($"Cache MISS: {cacheKey}");
            return false;
        }

        public void Set(string cacheKey, List<News> news)
        {
            // Begränsa cache-storleken
            if (_cache.Count >= _maxCacheSize)
            {
                ClearOldest();
            }

            var entry = new CacheEntry
            {
                News = news,
                ExpiresAt = DateTime.UtcNow.Add(_defaultExpiration)
            };

            _cache[cacheKey] = entry;
            System.Diagnostics.Debug.WriteLine($"Cache SET: {cacheKey} ({news.Count} articles, expires at {entry.ExpiresAt:HH:mm:ss})");
        }

        public void Clear()
        {
            _cache.Clear();
            System.Diagnostics.Debug.WriteLine("Cache cleared completely");
        }

        public void ClearExpired()
        {
            var now = DateTime.UtcNow;
            var expiredKeys = _cache
                .Where(x => x.Value.ExpiresAt <= now)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.TryRemove(key, out _);
            }

            if (expiredKeys.Any())
            {
                System.Diagnostics.Debug.WriteLine($"Cleared {expiredKeys.Count} expired cache entries");
            }
        }

        private void ClearOldest()
        {
            var oldest = _cache
                .OrderBy(x => x.Value.ExpiresAt)
                .Take(_cache.Count - _maxCacheSize + 10) // Ta bort 10 extra för att undvika frequent clearing
                .Select(x => x.Key)
                .ToList();

            foreach (var key in oldest)
            {
                _cache.TryRemove(key, out _);
            }

            System.Diagnostics.Debug.WriteLine($"Cleared {oldest.Count} oldest cache entries");
        }

        public string GenerateCacheKey(string endpoint, string? query, string? language, string? source, int page)
        {
            var normalizedQuery = string.IsNullOrWhiteSpace(query) ? "default" : query.Trim().ToLowerInvariant();
            var normalizedLanguage = language?.ToLowerInvariant() ?? "none";
            var normalizedSource = source?.ToLowerInvariant() ?? "none";

            return $"{endpoint}_{normalizedQuery}_{normalizedLanguage}_{normalizedSource}_{page}";
        }
    }
}
