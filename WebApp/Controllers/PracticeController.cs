using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using WebApp.Models.Common;
using WebApp.Models.ViewModels;
using WebApp.DB;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class PracticeController : Controller
    {
        readonly MSDevContext _context;
        public PracticeController(MSDevContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            if (id == null) return NotFound();
            var practice = _context.Practice.Find(id);
            if (practice == null) return NotFound();

            practice.Views++;
            _context.Update(practice);
            _context.SaveChanges();
            return View(practice);
        }
    }
}
