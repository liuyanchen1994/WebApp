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
using WebApp.Helpers;
using WebApp.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly MSDevContext _context;
        readonly IOptions<CognitiveOptions> CognitveOptions;
        public HomeController(MSDevContext context,
            IOptions<CognitiveOptions> options)
        {
            _context = context;
            CognitveOptions = options;
        }
        public IActionResult Index()
        {
            //获取站内新闻 
            var selfNews = _context.Blog
                .Where(m => m.Catalog.Value.Equals("articleSelfNews"))
                .Where(m => m.Status.Equals(StatusType.Publish))
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

            Console.WriteLine(keyword);
            //搜索资源 (文档、下载)
            var resource = _context.Resource
                .OrderByDescending(m => m.CreatedTime)
                .Where(m => m.Tag.Contains(keyword) || m.Name.Contains(keyword))
                .Take(20)
                .Include(m => m.Catalog)
                  .ThenInclude(m => m.TopCatalog)
                .ToList();

            //搜索视频 
            var search = new BingCustomSearchServices(CognitveOptions);
            var videos = search.SearchVideo(keyword, count: 20)?
                .Where(v => v.OpenGraphImage != null)
                .Take(10)
                .ToList();

            var answers = search.SearchQuestion(keyword, "zh-CN", count: 20);
            return View(
                new SearchViewModel
                {
                    ResourceList = resource,
                    VideoList = videos,
                    AnswerList = answers
                });
        }


        public IActionResult SearchVideoJump(string url)
        {
            if (url.Contains("channel9.msdn.com"))
            {
                var c9Video = _context.C9videos
                    .Where(m => m.SourceUrl.ToLower().Equals(url.Replace("https://channel9.msdn.com", "").ToLower()))
                    .FirstOrDefault();
                if (c9Video != null)
                {
                    Console.WriteLine(c9Video.Id);
                    return RedirectToAction(
                        nameof(VideoController.C9Detail),
                        "Video",
                        new { id = c9Video.Id }
                    );
                }
            }
            else if (url.Contains("mva.microsoft.com"))
            {
                var video = _context.MvaVideos
                    .Where(m => m.SourceUrl.ToLower().Equals(url.ToLower()))
                    .FirstOrDefault();
                if (video != null)
                {
                    return RedirectToAction(
                        nameof(VideoController.MvaDetail),
                        "Video",
                        new { id = video.Id }
                    );
                }
            }
            return Redirect(url);
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
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Test()
        {
            var search = new BingCustomSearchServices(CognitveOptions);
            ViewBag.Result = search.SearchQuestion("vs2017 代码风格");
            return View();
        }
    }
}
