using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.DB;
using WebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using WebApp.Helpers;
using WebApp.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace WebApp.Areas.Mobile.Controllers
{
    public class HomeController : MobileController
    {
        public HomeController(MSDevContext context):base(context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return RedirectToAction(nameof(NewsController.Index), "news");
        }

    }
}
