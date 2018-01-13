using System.Collections.Generic;
using WebApp.DB;

namespace WebApp.Areas.Mobile.Models
{
    public class DocumentViewModel
    {
        public IEnumerable<Catalog> Catalog { get; set; }

        public IEnumerable<Resource> Resource { get; set; }
    }
}
