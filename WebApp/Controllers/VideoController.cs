using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    public class VideoController : Controller
    {
        readonly MSDevContext _context;

        public VideoController(MSDevContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string tech)
        {
            var mvaVideos = new List<MvaVideos>();
            if (!string.IsNullOrEmpty(tech))
            {
                mvaVideos = _context.MvaVideos
                    .OrderByDescending(m => m.UpdatedTime)
                    .Where(m => m.Technologies.Contains(tech.ToLower()))
                    .Where(m => m.LanguageCode.Equals("zh-cn"))
                    .ToList();
            }
            else
            {
                mvaVideos = _context.MvaVideos
                    .OrderByDescending(m => m.UpdatedTime)
                    .Where(m=>m.LanguageCode.Equals("zh-cn"))
                    .Take(10)
                    .ToList();
            }

            var catalog = await _context.CataLog
                .Where(m => m.Type == "视频" && m.IsTop == 1)
                .Include(m => m.InverseTopCatalog)
                .ToArrayAsync();

            var documentList = new VideoViewModels
            {
                Catalog = catalog,
                MvaVideos = mvaVideos
            };
            return View(documentList);
        }

    }
}
