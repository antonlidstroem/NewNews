using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using SQLite;

namespace NewNews.DAL.Models
{

    public class SavedSearch
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Keyword { get; set; } = string.Empty;

        public string? Language { get; set; } = "sv";

        public string? Category { get; set; } = null; // bara för engelska

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
