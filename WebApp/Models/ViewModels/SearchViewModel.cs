using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DB;
using WebApp.Services;

namespace WebApp.Models.ViewModels
{
    public class SearchViewModel
    {
        public List<Resource> ResourceList { get; set; }
        public List<BingSearchWebPage> VideoList { get; set; }
        public List<BingSearchWebPage> AnswerList { get; set; }

    }
}
