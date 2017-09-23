using System.Collections.Generic;
using WebApp.DB;

namespace WebApp.Models.ViewModels
{
    public class NewsViewModels
    {

        public List<BingNews> BingNewsList { get; set; }
        public List<string> Tags { get; set; }

    }
}