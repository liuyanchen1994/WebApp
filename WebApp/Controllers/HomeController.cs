using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.DB;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly MSDevContext _context;
        public HomeController(MSDevContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var news = _context.BingNews
                .OrderByDescending(m => m.CreatedTime)
                .Take(8)
                .ToList();

            var msBlogs = _context.RssNews
                .OrderByDescending(m => m.LastUpdateTime)
                .Take(6)
                .ToList();

            var mvaVideos = _context.MvaVideos
                 .OrderByDescending(m => m.UpdatedTime)
                 .Where(m => m.IsRecommend)
                 .Where(m => m.LanguageCode.Equals("zh-cn"))
                 .Take(3)
                 .ToList();

            var downloads = _context.Resource
                .Where(m => m.Catalog.Type.Equals("下载"))
                .Where(m => m.IsRecommend == true)
                .OrderByDescending(m => m.UpdatedTime)
                .ToList();

            var documents = _context.Resource
                .Where(m => m.Catalog.Type.Equals("文档"))
                .Where(m => m.IsRecommend == true)
                .OrderByDescending(m => m.UpdatedTime)
                .ToList();


            var data = new IndexViewModels()
            {
                BingNews = news,
                MsBlogs = msBlogs,
                Downloads = downloads,
                Documents = documents,
                MvaVideos = mvaVideos
            };
            return View(data);
        }


        public IActionResult About()
        {

            return View();
        }

        [Route("/404")]
        public IActionResult Page404()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
