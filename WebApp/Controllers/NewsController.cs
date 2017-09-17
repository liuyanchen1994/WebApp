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

namespace WebApp.Controllers
{
    public class NewsController : Controller
    {
        readonly MSDevContext _context;
        public NewsController(MSDevContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = new NewsViewModels
            {
                BingNewsList = _context.BingNews.OrderByDescending(m => m.CreatedTime)
                   .Take(20)
                   .ToList()
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
