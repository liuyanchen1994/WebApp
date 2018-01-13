using System.Collections.Generic;
using WebApp.DB;

namespace WebApp.Areas.Mobile.Models
{
    public class VideoViewModel
    {
        /// <summary>
        /// 视频列表
        /// </summary>
        public List<Video> VideoList { get; set; }

        /// <summary>
        /// 系列目录
        /// </summary>
        public List<Catalog> SeriesList { get; set; }

        /// <summary>
        /// 当前目录
        /// </summary>
        public Catalog CurrentSeries { get; set; }

    }
}