using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.ViewModels;
using System.Collections.Generic;
using WebApp.Helpers;
using System;

namespace WebApp.Controllers
{
    public class VideoController : Controller
    {
        readonly MSDevContext _context;

        public VideoController(MSDevContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string tech, int p = 1)
        {
            int pageSize = 12;
            int totalNumber = 1;
            if (p < 1) p = 1;
            var mvaVideos = new List<MvaVideos>();
            if (!string.IsNullOrEmpty(tech))
            {
                mvaVideos = _context.MvaVideos
                    .OrderByDescending(m => m.UpdatedTime)
                    .Where(m => m.Tags.Contains(tech.ToLower()) || m.Title.Contains(tech.ToLower()))
                    .Where(m => m.LanguageCode.Equals("zh-cn"))
                    .Skip((p - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                totalNumber = _context.MvaVideos
                    .OrderByDescending(m => m.UpdatedTime)
                    .Where(m => m.Technologies.Contains(tech.ToLower()))
                    .Where(m => m.LanguageCode.Equals("zh-cn"))
                    .Count();
            }
            else
            {
                mvaVideos = _context.MvaVideos
                    .OrderByDescending(m => m.UpdatedTime)
                    .Where(m => m.LanguageCode.Equals("zh-cn"))
                    .Skip((p - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                totalNumber = _context.MvaVideos
                    .OrderByDescending(m => m.UpdatedTime)
                    .Where(m => m.LanguageCode.Equals("zh-cn"))
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
                RouteUrl = "/Video/Index",
                Total = totalNumber
            };

            var videoList = new VideoViewModels
            {
                Catalog = catalog,
                MvaVideos = mvaVideos,
                Pager = pageOption
            };

            return View(videoList);
        }

        [HttpGet]
        public IActionResult Detail(string id, string detail = null)
        {
            var video = _context.MvaVideos
                .Include(m => m.Details)
                .Where(m => m.Id == Guid.Parse(id))
                .FirstOrDefault();
            if (video.Details.Count < 1)
            {
                return RedirectToAction("Index");
            }
            var currentDetail = video.Details.OrderBy(m => m.Sequence).FirstOrDefault();

            if (!string.IsNullOrEmpty(detail))
            {
                currentDetail = _context.MvaDetails.Where(m => m.MvaId.Equals(detail)).FirstOrDefault();
            }
            return View(new VideoDetailModels
            {
                MvaVideo = video,
                Details = video.Details.OrderBy(m => m.Sequence).ToList(),
                CurrentDetail = currentDetail
            });
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
