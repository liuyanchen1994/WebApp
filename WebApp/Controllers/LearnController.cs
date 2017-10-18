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
        public async Task<IActionResult> Index()
        {
            var videoCatalogs = _context.CataLog.Where(m => m.Type.Equals("视频") && m.IsTop == 1).ToList();
            var articleCatalogs = _context.CataLog.Where(m => m.Type.Equals("文章") && m.IsTop == 1).ToList();

            return View(new LearnViewModels
            {
                VideoCatalogs = videoCatalogs,
                ArticleCatalogs = articleCatalogs
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
