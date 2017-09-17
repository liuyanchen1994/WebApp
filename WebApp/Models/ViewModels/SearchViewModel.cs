using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DB;

namespace WebApp.Models.ViewModels
{
    public class SearchViewModel
    {
        public List<Resource> ResourceList { get; set; }
        public List<C9videos> C9VideoList { get; set; }
        public List<MvaVideos> MvaVideoList { get; set; }

    }
}
