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

namespace WebApp.Areas.Mobile.Controllers
{
    public class NewsController : Controller
    {
        readonly MSDevContext _context;
        public NewsController(MSDevContext context)
        {
            _context = context;
        }
        public IActionResult Index(string tag)
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

            var data = new NewsViewModels
            {
                BingNewsList = query
                   .Take(20)
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
