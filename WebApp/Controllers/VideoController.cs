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

    public class VideoController : Controller
    {
        readonly MSDevContext _context;

        public VideoController(MSDevContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string tech, string type = "mva", int p = 1)
        {
            return NotFound();
            #region deprecated
            int pageSize = 6;
            int totalNumber = 1;
            if (p < 1) p = 1;
            var mvaVideos = new List<MvaVideos>();
            var c9Videos = new List<C9Videos>();

            var language = TempData["language"] ?? "";
            TempData.Keep();

            //判断视频类型
            if (type.Equals("mva"))
            {
                var query = _context.MvaVideos
                    .OrderByDescending(m => m.UpdatedTime)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(tech))
                {
                    query = query.Where(m => m.Tags.Contains(tech.ToLower()) || m.Title.Contains(tech.ToLower()));
                }

                if (language.Equals("zh-cn"))
                {
                    query = query.Where(m => m.LanguageCode.Equals("zh-cn"));
                }
                mvaVideos = query
                  .Skip((p - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();

                totalNumber = query
                    .Count();

            }
            else if (type.Equals("c9"))
            {
                var query = _context.C9videos
                    .OrderByDescending(m => m.UpdatedTime)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(tech))
                {
                    query = query.Where(m => m.Tags.Contains(tech.ToLower()) || m.Title.Contains(tech.ToLower()));
                }

                if (language.Equals("zh-cn"))
                {
                    query = query.Where(m => m.Language.Equals("zh-cn"));
                }
                c9Videos = query
                  .Skip((p - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();

                totalNumber = query
                    .Count();
            }

            var catalog = await _context.CataLog
                .Where(m => m.Type == "视频" && m.IsTop == 1)
                .Include(m => m.InverseTopCatalog)
                .ToArrayAsync();

            var pageOption = new MyPagerOption()
            {
                CurrentPage = p,
                PageSize = pageSize,
                RouteUrl = "/Video/Index" + "?tech=" + WebUtility.UrlEncode(tech) + "&type=" + WebUtility.UrlEncode(type),
                Total = totalNumber
            };

            var videoList = new VideoViewModels
            {
                Catalog = catalog,
                MvaVideos = mvaVideos,
                C9Videos = c9Videos,
                Pager = pageOption
            };
            return View(videoList);
            #endregion
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            var video = _context.Video.Find(Guid.Parse(id));
            video.Views++;
            _context.Update(video);
            _context.SaveChanges();
            return View(video);
        }

        [HttpGet]
        public IActionResult MvaDetail(string id, string detail = null)
        {
            var video = _context.MvaVideos
                .Where(m => m.Id == Guid.Parse(id))
                .Include(m => m.Details)
                .FirstOrDefault();

            if (video == null)
            {
                return NotFound();
            }
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
