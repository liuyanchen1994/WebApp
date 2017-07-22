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
    public class DocumentController : Controller
    {
        readonly MSDevContext _context;

        public DocumentController(MSDevContext context)
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
                    .Where(m=>m.Catalog.Type.Equals("文档"))
                    .Take(10)
                    .ToList();
            }

            var catalog = await _context.CataLog
                .Where(m => m.Type == "文档" && m.IsTop == 1)
                .Include(m => m.InverseTopCatalog)
                .ToArrayAsync();

            var documentList = new DocumentViewModels
            {
                Catalog = catalog,
                Resource = resource
            };
            return View(documentList);
        }

    }
}
