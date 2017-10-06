using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.DB;
using WebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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
            //获取站内新闻 
            var selfNews = _context.Blog
                .Where(m => m.Catalog.Value.Equals("articleSelfNews"))
                .OrderByDescending(m => m.UpdateTime)
                .Take(4)
                .ToList();

            var news = _context.BingNews
                .Where(m => m.Tags.Equals("微软"))
                .OrderByDescending(m => m.CreatedTime)
                .Take(8)
                .ToList();

            var msBlogs = _context.RssNews
                .OrderByDescending(m => m.LastUpdateTime)
                .Take(8)
                .ToList();

            var mvaVideos = _context.MvaVideos
                 .OrderByDescending(m => m.UpdatedTime)
                 .Where(m => m.IsRecommend)
                 .Where(m => m.LanguageCode.Equals("zh-cn"))
                 .Take(3)
                 .ToList();

            var c9Videos = _context.C9videos
                 .OrderByDescending(m => m.UpdatedTime)
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
                MvaVideos = mvaVideos,
                C9Videos = c9Videos,
                SelfNews = selfNews
            };
            return View(data);
        }


        [HttpGet]
        /// <summary>
        /// 搜索页内容
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IActionResult Search(string keyword)
        {

            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 3)
            {
                return RedirectToAction("Index");
            }
            //搜索资源 (文档、下载)
            var resource = _context.Resource
                .OrderByDescending(m => m.CreatedTime)
                .Where(m => m.Tag.Contains(keyword) || m.Name.Contains(keyword))
                .Take(20)
                .Include(m => m.Catalog)
                  .ThenInclude(m => m.TopCatalog)
                .ToList();
            //搜索视频 
            var c9videos = _context.C9videos
                .OrderByDescending(m => m.CreatedTime)
                .Where(m => m.Title.Contains(keyword))
                .Take(10)
                .ToList();

            var mvaVideos = _context.MvaVideos
                .OrderByDescending(m => m.CreatedTime)
                .Where(m => m.Title.Contains(keyword))
                .Take(6)
                .ToList();

            return View(
                new SearchViewModel
                {
                    ResourceList = resource,
                    C9VideoList = c9videos,
                    MvaVideoList = mvaVideos
                });
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
