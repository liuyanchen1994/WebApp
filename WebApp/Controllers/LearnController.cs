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

namespace WebApp.Controllers
{
    public class LearnController : Controller
    {
        readonly MSDevContext _context;

        public LearnController(MSDevContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(string topCatalogId = "", string navId = "", string type = "", int p = 1)
        {
            var language = TempData["language"]?.ToString() ?? "all";
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
                RouteUrl = Request.Path + $"?navId={navId}&type={type}&topCatalogId={topCatalogId}"
            };
            //博客一级目录
            var videoCatalogs = _context.CataLog.Where(m => m.Type.Equals("视频") && m.IsTop == 1).ToList();
            //视频一级目录
            var articleCatalogs = _context.CataLog.Where(m => m.Type.Equals("文章") && m.IsTop == 1).ToList();

            var secondaryNav = new List<CataLog>(); //左侧二级目录
            var blogList = new List<Blog>();
            var videoList = new List<Video>();
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
                    TempData["DetailPage"] = "MvaDetail";
                    videoList = _context.MvaVideos
                        .Where(m => language.Equals("all") || m.LanguageCode.Equals(language))
                        .OrderByDescending(m => m.CreatedTime)
                        .Skip((p - 1) * pageSize)
                        .Take(pageSize)
                        .Select(s =>
                        new Video
                        {
                            Id = s.Id,
                            Description = s.Description,
                            Author = s.Author,
                            CreatedTime = s.CreatedTime,
                            Duration = s.CourseDuration,
                            IsRecommend = s.IsRecommend,
                            Name = s.Title,
                            Tags = s.Tags,
                            ThumbnailUrl = s.CourseImage,
                            Url = s.SourceUrl,
                            Views = s.Views
                        }).ToList();
                    secondaryNav = _context.CataLog.Where(m => m.IsTop == 1 && m.Name.Equals("MVA"))
                        .Include(m => m.InverseTopCatalog)
                        .FirstOrDefault()
                        .InverseTopCatalog
                        .ToList();
                    //总数量
                    pageOption.Total = _context.MvaVideos
                        .Where(m => language.Equals("all") || m.LanguageCode.Equals(language)).Count();
                }
                else
                {
                    secondaryNav = _context.CataLog.Where(m => m.Id == Guid.Parse(topCatalogId))
                        .Include(m => m.InverseTopCatalog)
                        .FirstOrDefault()
                        .InverseTopCatalog
                        .ToList();
                }
                //默认的navId，根据当前catalogId获取
                if (String.IsNullOrEmpty(navId))
                {
                    navId = secondaryNav.FirstOrDefault()?.Id.ToString();
                }
                if (!string.IsNullOrWhiteSpace(navId))
                {
                    //判断分类类型
                    var catalog = _context.CataLog
                        .Where(m => m.Id == Guid.Parse(navId))
                        .Include(m => m.TopCatalog)
                        .FirstOrDefault();
                    if (catalog.Type.Equals("视频"))
                    {
                        switch (catalog.TopCatalog.Value)
                        {
                            case "mva":
                                TempData["DetailPage"] = "MvaDetail";
                                videoList = _context.MvaVideos.Where(m => m.Title.Contains(catalog.Name))
                                    .Where(m => language.Equals("all") || m.LanguageCode.Equals(language))
                                    .OrderByDescending(m => m.UpdatedTime)
                                    .Skip((p - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(s =>
                                    new Video
                                    {
                                        Id = s.Id,
                                        Description = s.Description,
                                        Author = s.Author,
                                        CreatedTime = s.CreatedTime,
                                        Duration = s.CourseDuration,
                                        IsRecommend = s.IsRecommend,
                                        Name = s.Title,
                                        Tags = s.Tags,
                                        ThumbnailUrl = s.CourseImage,
                                        Url = s.SourceUrl,
                                        Views = s.Views
                                    }).ToList();

                                pageOption.Total = _context.MvaVideos.Where(m => m.Title.Contains(catalog.Name))
                                    .Where(m => language.Equals("all") || m.LanguageCode.Equals(language))
                                    .Count();
                                break;
                            case "c9":
                                TempData["DetailPage"] = "C9Detail";
                                videoList = _context.C9videos.Where(m => m.Title.Contains(catalog.Name))
                                    .Where(m => language.Equals("all") || m.Language.Equals(language))
                                    .OrderByDescending(m => m.CreatedTime)
                                    .Skip((p - 1) * pageSize)
                                    .Take(pageSize)
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
                                    }).ToList();

                                pageOption.Total = _context.C9videos.Where(m => m.Title.Contains(catalog.Name))
                                    .Where(m => language.Equals("all") || m.Language.Equals(language))
                                    .Count();
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
                pageOption.Total = blogList.Count();

            }
            #endregion

            return View(new LearnViewModels
            {
                VideoCatalogs = videoCatalogs,
                ArticleCatalogs = articleCatalogs,
                SecondaryNavs = secondaryNav,
                BlogList = blogList,
                VideoList = videoList,
                Pager = pageOption
            });
        }

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetFilter(string language, string topCatalogId, string navId, string learnType)
        {
            TempData["language"] = language;

            return RedirectToAction(nameof(Index), new
            {
                topCatalogId = topCatalogId,
                navId = navId,
                type = learnType
            });
        }

        [HttpGet]
        public IActionResult Detail(string id, string detail = null)
        {


            var video = _context.MvaVideos
                .Include(m => m.Details)
                .Where(m => m.Id == Guid.Parse(id))
                .FirstOrDefault();
            //更新浏览数量
            video.Views++;
            _context.MvaVideos.Update(video);
            _context.SaveChanges();

            if (video.Details.Count < 1)
            {
                //重新获取详细内容
                return NotFound();
            }
            var currentDetail = video.Details.OrderBy(m => m.Sequence).FirstOrDefault();

            if (!string.IsNullOrEmpty(detail))
            {
                currentDetail = _context.MvaDetails.Where(m => m.MvaId.Equals(detail)).FirstOrDefault();
            }
            return View(new VideoDetailModels
            {
                MvaVideo = video,
                Details = video.Details.OrderBy(m => m.Sequence).ToList() ?? default,
                CurrentDetail = currentDetail ?? default
            });
        }

        [HttpGet]
        public IActionResult C9Detail(string id)
        {
            var video = _context.C9videos
                .Where(m => m.Id == Guid.Parse(id))
                .FirstOrDefault();

            //更新浏览数量
            video.Views++;
            _context.C9videos.Update(video);
            _context.SaveChanges();
            return View(video);
        }


        [HttpGet]
        public IActionResult SetLanguage(string language)
        {
            TempData["language"] = language;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult C9Video()
        {
            var re = _context.C9videos.Where(m => m.Language.Equals("zh-cn"))
                .Where(m => m.Duration != null)
                .Where(m => m.SeriesTitleUrl.Contains("Event"))
                .OrderByDescending(m => m.UpdatedTime)
                .Take(100)
                .ToList();
            return Json(re);

        }



    }
}
