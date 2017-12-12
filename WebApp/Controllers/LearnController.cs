using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.ViewModels;
using System.Collections.Generic;
using WebApp.Helpers;
using System;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Web;

namespace WebApp.Controllers
{
    public class LearnController : Controller
    {
        readonly MSDevContext _context;
        readonly ILogger<LearnController> _logger;

        public LearnController(MSDevContext context, ILogger<LearnController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="topCatalogId">顶级导航id</param>
        /// <param name="navId">二级导航id</param>
        /// <param name="type">内容类型</param>
        /// <param name="topicName">event主题名称</param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(string topCatalogId = "", string navId = "", string type = "", string eventId = "", int p = 1)
        {
            var language = TempData["language"]?.ToString() ?? "";
            if (!string.IsNullOrEmpty(type))
            {
                TempData["learnType"] = type;
            }
            else
            {
                type = "Video";
            }
            TempData.Keep();

            int pageSize = 12;
            var pageOption = new MyPagerOption
            {
                CurrentPage = p,
                PageSize = pageSize,
                RouteUrl = Request.Path + $"?navId={navId}&type={type}&topCatalogId={topCatalogId}&eventId={eventId}"
            };
            //博客一级目录
            var videoCatalogs = _context.CataLog.Where(m => m.Type.Equals("视频") && m.IsTop == 1).ToList();
            //视频一级目录
            var articleCatalogs = _context.CataLog.Where(m => m.Type.Equals("文章") && m.IsTop == 1).ToList();

            var secondaryNav = new List<Catalog>(); //左侧二级目录
            var blogList = new List<Blog>();
            var videoList = new List<Video>();
            var eventList = new List<C9Event>();//大会视频主题

            #region 查询视频内容
            if (type.Equals("Video"))
            {
                //默认主页显示内容
                if (String.IsNullOrWhiteSpace(topCatalogId))
                {
                    //videoList = _context.Video.Skip((page - 1) * pageSize)
                    //   .Take(pageSize)
                    //   .ToList();
                    //secondaryNav = _context.CataLog.Where(m => m.IsTop == 1 && m.Name.Equals("视频教程"))
                    //    .Include(m => m.InverseTopCatalog)
                    //    .FirstOrDefault()
                    //    .InverseTopCatalog
                    //    .ToList();
                    secondaryNav = _context.CataLog.Where(m => m.IsTop == 1 && m.Value.Equals("CourseVideo"))
                        .Include(m => m.InverseTopCatalog)
                        .FirstOrDefault()?
                        .InverseTopCatalog?
                        .ToList();
                }
                else
                {
                    secondaryNav = _context.CataLog.Where(m => m.Id == Guid.Parse(topCatalogId))
                        .Include(m => m.InverseTopCatalog)
                        .FirstOrDefault()?
                        .InverseTopCatalog?
                        .ToList();
                }
                //默认的navId，根据当前catalogId获取
                if (String.IsNullOrEmpty(navId))
                {
                    navId = secondaryNav.FirstOrDefault()?.Id.ToString();
                }
                if (!String.IsNullOrEmpty(navId))
                {
                    //判断分类类型
                    var catalog = _context.CataLog
                        .Where(m => m.Id == Guid.Parse(navId))
                        .Include(m => m.TopCatalog)
                        .FirstOrDefault();
                    if (catalog.Type.Equals("视频"))
                    {
                        string searchKey = "";//搜索的关键词
                        string languageWhere = string.IsNullOrWhiteSpace(language) ? "%" : language + "%";//语言条件

                        switch (catalog.TopCatalog.Value.ToLower())
                        {
                            case "event":
                                TempData["DetailPage"] = "EventDetail";
                                //大会类型
                                string catalogValue = catalog.Value.Replace("Event", "");
                                //查询大会目录
                                eventList = _context.C9Event
                                   .Where(m => m.EventName.Equals(catalogValue))
                                   .ToList();
                                //默认选择项
                                if (string.IsNullOrEmpty(eventId))
                                {
                                    eventId = eventList.FirstOrDefault()?.Id.ToString();
                                }
                                videoList = _context.EventVideo
                                    .Where(m => m.C9EventId.ToString().Equals(eventId))
                                    .OrderByDescending(m => m.CreatedTime)
                                    .Select(s =>
                                    new Video
                                    {
                                        Id = s.Id,
                                        Description = s.Description,
                                        Author = s.Author,
                                        CreatedTime = s.CreatedTime,
                                        Duration = s.Duration,
                                        IsRecommend = false,
                                        Name = s.Title,
                                        Tags = s.Tags,
                                        ThumbnailUrl = s.ThumbnailUrl,
                                        Url = s.SourceUrl,
                                        Views = s.Views
                                    })
                                    .Skip((p - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

                                pageOption.Total = _context.EventVideo
                                    .Where(m => m.C9EventId.ToString().Equals(eventId))
                                    .Count();
                                break;

                            case "mva":
                                TempData["DetailPage"] = "MvaDetail";
                                searchKey = $"\"{catalog.Name}\"";
                                var mvaVideos = _context.MvaVideos
                                    .FromSql($@"
                                        SELECT * FROM MvaVideos WHERE contains(Title, {searchKey})
                                        AND LanguageCode LIKE {languageWhere}
                                        ORDER BY UpdatedTime DESC
                                        OFFSET {(p - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY
                                    ")
                                    .ToList();

                                videoList = mvaVideos.Select(s =>
                                     new Video
                                     {
                                         Id = s.Id,
                                         Description = s.Description,
                                         Author = s.Author,
                                         CreatedTime = s.CreatedTime,
                                         Duration = s.CourseDuration,
                                         IsRecommend = false,
                                         Name = s.Title,
                                         Tags = s.Tags,
                                         ThumbnailUrl = s.CourseImage,
                                         Url = s.SourceUrl,
                                         Views = s.Views
                                     }).ToList();

                                pageOption.Total = _context.MvaVideos
                                    .FromSql($@"
                                        SELECT * FROM MvaVideos WHERE contains(Title, {searchKey})
                                        AND LanguageCode LIKE {languageWhere}
                                    ")
                                    .Count();
                                break;
                            case "c9":
                                TempData["DetailPage"] = "C9Detail";
                                searchKey = $"\"{catalog.Name}\"";


                                var c9videos = _context.C9videos
                                    .FromSql($@"
                                        SELECT * FROM C9Videos WHERE contains(Title, {searchKey})
                                        AND Language LIKE {languageWhere}
                                        ORDER BY UpdatedTime DESC
                                        OFFSET {(p - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY
                                    ")
                                    .ToList();
                                videoList = c9videos
                                    .Select(s => new Video
                                    {
                                        Id = s.Id,
                                        Description = s.Description,
                                        Author = s.Author,
                                        CreatedTime = s.CreatedTime,
                                        Duration = s.Duration,
                                        IsRecommend = false,
                                        Name = s.Title,
                                        Tags = s.Tags,
                                        ThumbnailUrl = s.ThumbnailUrl,
                                        Url = s.SourceUrl,
                                        Views = s.Views
                                    }).ToList();


                                pageOption.Total = _context.C9videos
                                    .FromSql($@"
                                        SELECT * FROM C9Videos WHERE contains(Title, {searchKey})
                                        AND Language LIKE {languageWhere}
                                    ")
                                    .Count();
                                _logger.LogDebug(pageOption.Total.ToString());
                                break;
                            default:
                                TempData["DetailPage"] = "Detail";
                                videoList = _context.Video.Where(m => m.Catalog == catalog)
                                    .OrderByDescending(m => m.UpdatedTime)
                                    .Skip((p - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

                                pageOption.Total = _context.Video.Where(m => m.Catalog == catalog)
                                    .Count();
                                break;
                        }
                    }
                    else if (catalog.Type.Equals("文章"))
                    {
                        blogList = _context.Blog.Where(m => m.Catalog == catalog).ToList();
                    }
                }
            }
            #endregion

            #region 查询博客内容
            if (type.Equals("Blog"))
            {
                //默认主页显示内容
                if (String.IsNullOrWhiteSpace(topCatalogId))
                {
                    secondaryNav = _context.CataLog.Where(m => m.IsTop == 1 && m.Value.Equals("ArticleCourse"))
                        .Include(m => m.InverseTopCatalog)
                        .FirstOrDefault()?
                        .InverseTopCatalog?
                        .ToList();
                }
                else
                {
                    secondaryNav = _context.CataLog.Where(m => m.Id == Guid.Parse(topCatalogId))
                        .Include(m => m.InverseTopCatalog)
                        .FirstOrDefault()?
                        .InverseTopCatalog?
                        .ToList();
                }
                //默认的navId，根据当前catalogId获取
                if (String.IsNullOrEmpty(navId))
                {
                    navId = secondaryNav.FirstOrDefault()?.Id.ToString();
                }
                if (!string.IsNullOrWhiteSpace(navId))
                {
                    blogList = _context.Blog.Where(m => m.Catalog.Id.ToString().Equals(navId))
                        .Where(m => m.Status.Equals(StatusType.Publish))
                        .ToList();
                }
                pageOption.Total = blogList
                    .Where(m => m.Status.Equals(StatusType.Publish))
                    .Count();

            }
            #endregion

            return View(new LearnViewModels
            {
                VideoCatalogs = videoCatalogs,
                ArticleCatalogs = articleCatalogs,
                SecondaryNavs = secondaryNav,
                BlogList = blogList,
                VideoList = videoList,
                Pager = pageOption,
                EventList = eventList
            });
        }

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetFilter(string language, string topCatalogId, string navId, string learnType, string eventId)
        {
            TempData["language"] = language;
            return RedirectToAction(nameof(Index), new
            {
                topCatalogId = topCatalogId,
                navId = navId,
                type = learnType,
                eventId = eventId
            });
        }

        [HttpGet]
        public IActionResult SetLanguage(string language)
        {
            TempData["language"] = language;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Blog(Guid id)
        {
            if (id == null) return NotFound();
            var blog = _context.Blog
                .Where(m => m.Id == id)
                .Include(m => m.Catalog)
                .Include(m => m.Video)
                .Include(m => m.Practice)
                .FirstOrDefault();
            if (blog == null) return NotFound();

            var relateBlogs = _context.Blog
                .Where(m => m.Catalog == blog.Catalog)
                .Where(m=>m.Status.Equals(StatusType.Publish))
                .Where(m => m.Id != blog.Id)
                .ToList();

            return View(new LearnBlogViewModel
            {
                Blog = blog,
                RelateBlogs = relateBlogs
            });
        }

        public IActionResult Test()
        {

            return Json("");
        }
    }
}
