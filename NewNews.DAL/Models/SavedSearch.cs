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

        // För att kunna filtrera på kategori i framtiden
        public string? Category { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
