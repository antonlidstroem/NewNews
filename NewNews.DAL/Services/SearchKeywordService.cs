using System;
using System.Collections.Generic;
using System.Text;
using NewNews.DAL.Models;
using SQLite;

namespace NewNews.DAL.Services
{
    public class SearchKeywordService
    {
        private readonly SQLiteAsyncConnection _db;

        public SearchKeywordService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<SavedSearch>().Wait();
        }

        public Task<List<SavedSearch>> GetAllKeywordsAsync()
        {
            return _db.Table<SavedSearch>()
                      .OrderByDescending(k => k.CreatedAt)
                      .ToListAsync();
        }

        public Task AddKeywordAsync(string keyword, string language, string? category = null)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Task.CompletedTask;

            return _db.ExecuteAsync(
                "INSERT OR IGNORE INTO SavedSearch (Keyword, Language, Category, CreatedAt) VALUES (?, ?, ?, ?)",
                keyword,
                language,
                category,
                DateTime.UtcNow
            );
        }
        public Task DeleteKeywordAsync(int id)
        {
            return _db.DeleteAsync<SavedSearch>(id);
        }



    }
}
