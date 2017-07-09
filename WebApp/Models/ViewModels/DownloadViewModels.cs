using System.Collections.Generic;
using WebApp.DB;

namespace WebApp.Models.ViewModels
{
    public class DownloadViewModels
    {
        public IEnumerable<CataLog> Catalog { get; set; }
    }
}
