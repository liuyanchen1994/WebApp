using System;
using System.Collections.Generic;

namespace WebApp.DB
{
    public partial class C9videos
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Language { get; set; }
        public string Mp3Url { get; set; }
        public string Mp4HigUrl { get; set; }
        public string Mp4LowUrl { get; set; }
        public string Mp4MidUrl { get; set; }
        public string SeriesTitle { get; set; }
        public string SeriesTitleUrl { get; set; }
        public string SourceUrl { get; set; }
        public int? Status { get; set; }
        public string Tags { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Title { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int? Views { get; set; }
    }
}
