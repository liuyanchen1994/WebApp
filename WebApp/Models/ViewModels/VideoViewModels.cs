using System.Collections.Generic;
using WebApp.DB;
using WebApp.Helpers;

namespace WebApp.Models.ViewModels
{
    public class VideoViewModels
    {
        public IEnumerable<CataLog> Catalog { get; set; }


        public MyPagerOption Pager { get; set; }
        public IEnumerable<Resource> Resource { get; set; }

        public IEnumerable<MvaVideos> MvaVideos { get; set; }
    }

    public class VideoDetailModels
    {
        public MvaVideos MvaVideo { get; set; }
        public List<MvaDetails> Details { get; set; }
    }
}
