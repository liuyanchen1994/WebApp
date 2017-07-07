using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using Microsoft.EntityFrameworkCore;

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
            var sources = _context.CataLog
                .Where(m => m.Type == "下载" && m.IsTop == 1)
                .Include(m => m.InverseTopCatalog)
                .ToList();

            return Json(sources);

        }

    }
}
