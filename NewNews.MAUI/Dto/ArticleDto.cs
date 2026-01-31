namespace NewNews.MAUI.Dto
{
    public class ArticleDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? Content { get; set; }
        public SourceDto Source { get; set; } = new();

        public string? Category { get; set; }
    }
}