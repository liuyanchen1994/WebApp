using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    public class DownloadController : Controller
    {
        readonly MSDevContext _context;

        public DownloadController(MSDevContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var sources = await _context.CataLog
                .Where(m => m.Type == "下载" && m.IsTop == 1)
                .Include(m => m.InverseTopCatalog)
                    .ThenInclude(catalog => catalog.Resource)
                .ToArrayAsync();

            var downloadList = new DownloadViewModels
            {
                Catalog = sources
            };
            return View(downloadList);
        }

    }
}
