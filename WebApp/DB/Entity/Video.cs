using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.DB
{
    /// <summary>
    /// 本站视频
    /// </summary>
    public class Video
    {
        public Guid Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public CataLog Catalog { get; set; }
        [MaxLength(256)]
        public string Url { get; set; }
        [MaxLength(16)]
        public string Duration { get; set; }
        [MaxLength(32)]
        public string Author { get; set; }
        [MaxLength(128)]
        public string Tags { get; set; }
        [MaxLength(32)]
        public string Status { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsRecommend { get; set; }
        public int? Views { get; set; }
    }
}
