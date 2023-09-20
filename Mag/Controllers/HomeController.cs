using Mag.Areas.Admin.Models.Dto.Home;
using Mag.Areas.Admin.Models.Dto.User;
using Mag.Common;
using Mag.Data;
using Mag.Models;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                Banner1PhotoText = baner1.PhotoText == null ? "" : baner1.PhotoText,
                Banner1PhotoLink = baner1.PhotoLink == null ? "" : baner1.PhotoLink,
                Banner2Address = baner2 == null ? "" : baner2.BannerAddress,
                Banner2PhotoText = baner2.PhotoText == null ? "" : baner2.PhotoText,
                Banner2PhotoLink = baner2.PhotoLink == null ? "" : baner2.PhotoLink,
                Banner3Address = baner3 == null ? "" : baner3.BannerAddress,
                Banner3PhotoText = baner3.PhotoText == null ? "" : baner3.PhotoText,
                Banner3PhotoLink = baner3.PhotoLink == null ? "" : baner3.PhotoLink,
                Banner4Address = baner4 == null ? "" : baner4.BannerAddress,
                Banner4PhotoText = baner4.PhotoText == null ? "" : baner4.PhotoText,
                Banner4PhotoLink = baner4.PhotoLink == null ? "" : baner4.PhotoLink,
                Banner5Address = baner5 == null ? "" : baner5.BannerAddress,
                Banner5PhotoText = baner5.PhotoText == null ? "" : baner5.PhotoText,
                Banner5PhotoLink = baner5.PhotoLink == null ? "" : baner5.PhotoLink,
                Banner6Address = baner6 == null ? "" : baner6.BannerAddress,
                Banner6PhotoText = baner6.PhotoText == null ? "" : baner6.PhotoText,
                Banner6PhotoLink = baner6.PhotoLink == null ? "" : baner6.PhotoLink,
                Banner7Address = baner7 == null ? "" : baner7.BannerAddress,
                Banner7PhotoText = baner7.PhotoText == null ? "" : baner7.PhotoText,
                Banner7PhotoLink = baner7.PhotoLink == null ? "" : baner7.PhotoLink,
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

        #region ShowProfile
        [HttpGet]
        [Route("{UserName}")]
        public async Task<IActionResult> ShowUserProfile(string UserName)
        {
            var FindUser = await _DbContext.Users.FirstOrDefaultAsync(p => p.UserName == UserName);
            if (FindUser == null)
            {
                return Redirect("NotFound/User");
            }
            return View();
        }
        #endregion

        #region NotFound
        [HttpGet]
        [Route("NotFound/User")]
        public IActionResult NotFound()
        {
            return View();
        }
        #endregion
    }
}