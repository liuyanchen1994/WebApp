using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DB;

namespace WebApp.Models.ViewModels
{
    public class IndexViewModels
    {
        public List<BingNews> BingNews { get; set; }

        public List<Resource> Downloads { get; set; }

    }
}
