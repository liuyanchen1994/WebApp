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
    public class CommunityController : MobileController
    {

        public CommunityController(MSDevContext context) : base(context)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Blog(Guid id)
        {

            if (id == null) return NotFound();
            var blog = _context.Blog
                .Where(m => m.Id == id)
                .Include(m => m.Catalog)
                .Include(m => m.Video)
                .Include(m => m.Practice)
                .FirstOrDefault();

            if (blog == null) return NotFound();

            if (blog.Video?.Status != StatusType.Publish)
            {
                blog.Video = default;
            }


            if (blog.Views == null)
            {
                blog.Views = 1;
            }
            else
            {
                blog.Views++;
            }
            _context.Update(blog);
            _context.SaveChangesAsync();

            return View(new LearnBlogViewModel
            {
                Blog = blog,
                RelateBlogs = default
            });
        }
    }
}
