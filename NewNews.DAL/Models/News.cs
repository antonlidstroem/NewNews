using CommunityToolkit.Mvvm.ComponentModel;

namespace NewNews.DAL.Models
{
    public class News
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? ImageUrl { get; set; }
        public string? Source { get; set; }
        public string? Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
