using System.Collections.Generic;
using WebApp.DB;

namespace WebApp.Models.ViewModels
{
    public class ArticleViewModels
    {
        public List<Blog> ArticleList { get; set; }
        public List<Catalog> CatalogList{ get; set; }
    }
}