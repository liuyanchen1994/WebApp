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
        public async Task<IActionResult> Index()
        {

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://api.msdev.cc/")
            };
            var re = await client.GetStringAsync("api/BingNews/PageList");
            var jsonRe = JsonConvert.DeserializeObject<ReturnJson<IEnumerable<BingNews>>>(re);

            var data = new NewsViewModels();
            if (jsonRe.ErrorCode == 0)
            {
                data.BingNewsList = jsonRe.Data.ToList();
            }
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
