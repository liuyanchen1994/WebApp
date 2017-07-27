using System.Collections.Generic;
using WebApp.DB;

namespace WebApp.Models.ViewModels
{
    public class VideoViewModels
    {
        public IEnumerable<CataLog> Catalog { get; set; }

        public IEnumerable<Resource> Resource { get; set; }

        public IEnumerable<MvaVideos> MvaVideos { get; set; }
    }
}
