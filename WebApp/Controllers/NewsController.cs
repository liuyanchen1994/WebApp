using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Net.Http;
using Newtonsoft.Json;
using WebApp.Models.Common;
using WebApp.Models.Entity;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    public class NewsController : Controller
    {
        public async Task<IActionResult> Index ()
        {

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://api.msdev.cc/")
            };
            var re = await client.GetStringAsync("api/BingNews/PageList");
            var jsonRe = JsonConvert.DeserializeObject<ReturnJson<IEnumerable<BingNews>>>(re);

            var data = new NewsViewModels();
            if (jsonRe.ErrorCode == 0)
            {
                data.BingNewsList = jsonRe.Data.ToList();
            }
            return View(data);
        }

    }
}
