using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Areas.Mobile.Models;

namespace WebApp.Areas.Mobile.Controllers
{
    public class NewsController : MobileController
    {
        public NewsController(MSDevContext context) : base(context)
        {
            _context = context;
        }
        public IActionResult Index(string tag = "微软")
        {
            var query = _context.BingNews.OrderByDescending(m => m.CreatedTime)
                .AsQueryable();

            var tags = _context.BingNews.Select(m => m.Tags)
                .Distinct()
                .ToList();

            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(m => m.Tags.Equals(tag));
            }

            var data = new NewsViewModel
            {
                BingNewsList = query
                   .Take(32)
                   .ToList(),
                Tags = tags
            };
            return View(data);
        }


        [HttpGet]
        public IActionResult RssNewsDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var news = _context.RssNews.Find(id);

            return View(news);
        }
    }
}
