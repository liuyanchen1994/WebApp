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
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public class ArticleController : Controller
    {
        readonly MSDevContext _context;
        public ArticleController(MSDevContext context)
        {
            _context = context;
        }
        public IActionResult Index(string catalog = "csharpQuickStart")
        {
            // 获取目录分类
            var topCatalogId = _context.CataLog.Where(m => m.Name.Equals("教程"))
                .Select(m => m.Id)
                .FirstOrDefault();

            var catalogs = _context.CataLog
                .Where(m => m.TopCatalogId == topCatalogId)
                .ToList();

            // 查询新闻 
            if (string.IsNullOrWhiteSpace(catalog))
            {
                catalog = catalogs.FirstOrDefault().Value;
            }
            var news = _context.Blog
                .Where(m => m.Catalog.Value.Equals(catalog))
                .OrderByDescending(m => m.CreatedTime)
                .ToList();

            return View(new ArticleViewModels
            {
                ArticleList = news,
                CatalogList = catalogs
            });
        }


        [HttpGet]
        [Route("/Article/{catalog:required}/{id:guid}", Name = "Article")]
        public IActionResult Detail(string catalog, Guid id)
        {

            if (id == null) return NotFound();
            var blog = _context.Blog
                .Where(m => m.Id == id)
                .Include(m => m.Catalog)
                .Include(m => m.Video)
                .Include(m => m.Practice)
                .FirstOrDefault();

            if (blog == null) return NotFound();

            if (blog.Video?.Status != StatusType.Publish)
            {
                blog.Video = default;
            }

            var relateBlogs = _context.Blog
                .Where(m => m.Catalog == blog.Catalog)
                .Where(m => m.Status.Equals(StatusType.Publish))
                .Where(m => m.Id != blog.Id)
                .ToList();

            if (blog.Views == null)
            {
                blog.Views = 1;
            }
            else
            {
                blog.Views++;
            }
            _context.Update(blog);
            _context.SaveChangesAsync();

            return View(new LearnBlogViewModel
            {
                Blog = blog,
                RelateBlogs = relateBlogs
            });
        }
    }
}
