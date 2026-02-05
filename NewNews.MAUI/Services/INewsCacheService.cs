using System;
using System.Collections.Generic;
using System.Text;
using NewNews.DAL.Models;

namespace NewNews.MAUI.Services
{
    public interface INewsCacheService
    {
        bool TryGet(string cacheKey, out List<News> news);
        void Set(string cacheKey, List<News> news);
        void Clear();
        void ClearExpired();
        string GenerateCacheKey(string endpoint, string? query, string? language, string? source, int page);
    }
}
