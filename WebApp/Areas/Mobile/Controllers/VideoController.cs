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
using WebApp.Areas.Mobile.Models;

namespace WebApp.Areas.Mobile.Controllers
{

    public class VideoController : MobileController
    {
        public VideoController(MSDevContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet]
        /// <summary>
        /// 视频主页
        /// </summary>
        /// <param name="id">系列id</param>
        /// <returns></returns>
        public IActionResult Index(Guid? id)
        {
            // 获取系列目录
            var series = _context.CataLog
                .Where(m => m.Value.Equals("CourseVideo"))
                .Include(m => m.TopCatalog)
                .ToList();

            // 获取默认系列
            var defaultVideo = _context.Config
                .Where(m => m.Name.Equals("defaultVideoSeries"))
                .FirstOrDefault();

            // 获取视频内容
            var catalog = new Catalog();
            if (id == null)
            {
                catalog = _context.CataLog
                    .Where(m => m.Value.Equals(defaultVideo.Value))
                    .FirstOrDefault();
            }
            else
            {
                catalog = _context.CataLog.Find(id);
            }

            var videos = _context.Video
                .Where(m => m.Catalog == catalog)
                .OrderByDescending(m => m.CreatedTime)
                .ToList();

            var data = new VideoViewModel
            {
                CurrentSeries = catalog,
                VideoList = videos,
                SeriesList = series
            };
            return View(data);
        }



        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var video = _context.Video
                .Where(m => m.Status == StatusType.Publish)
                .Where(m => m.Id == id)
                .Include(m => m.Blog)
                .Include(m => m.Practice)
                .FirstOrDefault();

            if (video.Views == null)
            {
                video.Views = 1;
            }
            else
            {
                video.Views++;
            }
            _context.Update(video);
            _context.SaveChanges();
            return View(video);
        }
    }
}
