using System.Collections.Generic;
using WebApp.DB;

namespace WebApp.Models.ViewModels
{
    public class DocumentViewModels
    {
        public IEnumerable<Catalog> Catalog { get; set; }

        public IEnumerable<Resource> Resource { get; set; }
    }
}
