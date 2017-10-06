using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using WebApp.Models.Common;
using WebApp.Models.ViewModels;
using WebApp.DB;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class ArticleController : Controller
    {
        readonly MSDevContext _context;
        public ArticleController(MSDevContext context)
        {
            _context = context;
        }
        public IActionResult Index(string catalog = "articleSelfNews")
        {
            //获取目录分类
            var topCatalog = _context.CataLog.Where(m => m.Name.Equals("资讯") && m.IsTop == 1)
                .Include(m => m.InverseTopCatalog)
                .FirstOrDefault();
            var catalogs = topCatalog.InverseTopCatalog.ToList();

            var currentCatalog = catalogs.Where(m => m.Value.Equals(catalog)).FirstOrDefault();

            //查询新闻 
            var news = _context.Blog
                .Where(m => m.Catalog == currentCatalog)
                .Skip(0)
                .Take(12)
                .ToList();

            return View(new ArticleViewModels
            {
                ArticleList = news,
                CatalogList = catalogs
            });
        }


        [HttpGet]
        public IActionResult Detail(string id)
        {
            if (id == null) return NotFound();
            var article = _context.Blog.Find(Guid.Parse(id));
            if (article == null) return NotFound();

            article.Views++;
            _context.Update(article);
            _context.SaveChanges();
            return View(article);
        }
    }
}
