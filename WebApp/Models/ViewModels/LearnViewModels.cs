using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DB;
using WebApp.Helpers;

namespace WebApp.Models.ViewModels
{
    public class LearnViewModels
    {
        public List<Catalog> VideoCatalogs { get; set; }
        public List<Catalog> ArticleCatalogs { get; set; }
        public List<Catalog> PracticeCatalogs { get; set; }
        /// <summary>
        /// Events Topic Name
        /// </summary>
        public List<C9Event> EventList { get; set; }
        public List<Catalog> SecondaryNavs { get; set; }

        public List<Video> VideoList { get; set; }
        public List<Blog> BlogList { get; set; }
        public MyPagerOption Pager{ get; set; }
    }

    public class LearnBlogViewModel
    {
        public Blog Blog { get; set; }
        public List<Blog> RelateBlogs { get; set; }
    }
}
