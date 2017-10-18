using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DB;

namespace WebApp.Models.ViewModels
{
    public class LearnViewModels
    {
        public List<CataLog> VideoCatalogs { get; set; }
        public List<CataLog> ArticleCatalogs { get; set; }
        public List<CataLog> PracticeCatalogs { get; set; }



    }
}
