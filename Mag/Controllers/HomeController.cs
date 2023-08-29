using Mag.Areas.Admin.Models.Dto.Home;
using Mag.Areas.Admin.Models.Dto.User;
using Mag.Common;
using Mag.Data;
using Mag.Models;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Mag.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataBaseContext _DbContext;
        private readonly UserManager<User> _userManager;
        public HomeController(DataBaseContext dataBaseContext, UserManager<User> userManager)
        {
            _DbContext = dataBaseContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var baner1 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "Banner1");
            var baner2 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "Banner2");
            var baner3 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "Banner3");
            var baner4 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "Banner4");
            var baner5 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "Banner5");
            var baner6 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "Banner6");
            var baner7 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "Banner7");

            var banner = new BannersDto
            {
                Banner1Address = baner1 == null ? "" : baner1.BannerAddress,
                Banner2Address = baner2 == null ? "" : baner2.BannerAddress,
                Banner3Address = baner3 == null ? "" : baner3.BannerAddress,
                Banner4Address = baner4 == null ? "" : baner4.BannerAddress,
                Banner5Address = baner5 == null ? "" : baner5.BannerAddress,
                Banner6Address = baner6 == null ? "" : baner6.BannerAddress,
                Banner7Address = baner7 == null ? "" : baner7.BannerAddress,
            };
            return View(banner);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}