using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApp.Areas.Mobile.Models;
using System;

namespace WebApp.Areas.Mobile.Controllers
{
    public class DocumentController : MobileController
    {
        public DocumentController(MSDevContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(Guid? id)
        {
            var resource = new List<Resource>();
            if (id != null)
            {
                resource = _context.Resource.Where(m => m.CatalogId == id)
                    .ToList();
            }
            else
            {
                resource = _context.Resource
                    .Where(m => m.Catalog.Type.Equals("文档"))
                    .ToList();
            }

            var catalog = await _context.CataLog
                .Where(m => m.Type.Equals("文档") && m.IsTop == 1)
                .Include(m => m.InverseTopCatalog)
                .ToArrayAsync();

            var documentList = new DocumentViewModel
            {
                Catalog = catalog,
                Resource = resource
            };
            return View(documentList);
        }

    }
}
