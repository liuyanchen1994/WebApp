using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DB;

namespace WebApp.Areas.Mobile.Models
{
    public class NewsViewModel
    {
        public List<BingNews> BingNewsList { get; set; }
        public List<string> Tags { get; set; }
    }
}
