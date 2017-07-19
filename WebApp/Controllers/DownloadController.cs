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
    public class DownloadController : Controller
    {
        readonly MSDevContext _context;

        public DownloadController(MSDevContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string id)
        {
            var resource = new List<Resource>();
            if (!string.IsNullOrEmpty(id))
            {
                resource = _context.Resource.Where(m => m.CatalogId.ToString().Equals(id.ToUpper()))
                    .ToList();
            }
            else
            {
                resource = _context.Resource
                    .Where(m=>m.Catalog.Type.Equals("下载"))
                    .Take(10)
                    .ToList();
            }

            var catalog = await _context.CataLog
                .Where(m => m.Type == "下载" && m.IsTop == 1)
                .Include(m => m.InverseTopCatalog)
                .ToArrayAsync();

            var downloadList = new DownloadViewModels
            {
                Catalog = catalog,
                Resource = resource
            };
            return View(downloadList);
        }

    }
}
