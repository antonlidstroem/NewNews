using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using NewNews.DAL.Models;

// Wrappar News-model + UI-state
namespace NewNews.MAUI.ViewModels
{
    public partial class ArticleViewModel : ObservableObject
    {
        public News Model { get; }

        public ArticleViewModel(News model)
        {
            Model = model;
        }

        public string? Title => Model.Title;
        public string? Description => Model.Description;
        public string? Url => Model.Url;
        public string? ImageUrl => Model.ImageUrl;
        public string? Source => Model.Source;
        //public string? Content => Model.Content;
        public DateTime PublishedAt => Model.PublishedAt;

        // UI-state
        [ObservableProperty]
        private bool isExpanded;

        [ObservableProperty]
        private double webViewHeight = 1;

        public string Content => Clean(Model.Content);

        private static string Clean(string? content)
        {
            if (string.IsNullOrEmpty(content)) return "";

            return content
                .Replace("<ul><li>", "")
                .Replace("</li></ul>", "");
        }

    }
}
