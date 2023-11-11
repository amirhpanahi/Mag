using Mag.Areas.Admin.Models.Dto.Ads;
using Mag.Areas.Admin.Models.Dto.Home;
using Mag.Areas.Admin.Models.Dto.User;
using Mag.Common;
using Mag.Data;
using Mag.Models;
using Mag.Models.Dto.Home;
using Mag.Models.Dto.Profile;
using Mag.Models.Entities;
using Mag.Services.FileUploadService;
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
        private readonly IFileUploadService _fileUpload;
        public HomeController(DataBaseContext dataBaseContext, UserManager<User> userManager,IFileUploadService fileUploadService)
        {
            _DbContext = dataBaseContext;
            _userManager = userManager;
            _fileUpload = fileUploadService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var baner1 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "MainBanner1"); 
            var baner2 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "MainBanner2");
            var baner3 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "MainBanner3");
            var baner4 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "MainBanner4");
            var baner5 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "MainBanner5");
            var baner6 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "MainBanner6");

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
            };

            //var Ads1 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP1");
            //var Ads2 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP2");
            //var Ads3 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP3");
            //var Ads4 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP4");
            //var Ads5 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP5");
            //var Ads6 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP6");

            //var Ads = new AdsDto
            //{
            //    Address1 = Ads1 == null ? "" : Ads1.Address,
            //    Alt1 = Ads1 == null ? "" : Ads1.Alt,
            //    Title1 = Ads1 == null ? "" : Ads1.Title,
            //    Address2 = Ads2 == null ? "" : Ads2.Address,
            //    Alt2 = Ads2 == null ? "" : Ads2.Alt,
            //    Title2 = Ads2 == null ? "" : Ads2.Title,
            //    Address3 = Ads3 == null ? "" : Ads3.Address,
            //    Alt3 = Ads3 == null ? "" : Ads3.Alt,
            //    Title3 = Ads3 == null ? "" : Ads3.Title,
            //    Address4 = Ads4 == null ? "" : Ads4.Address,
            //    Alt4 = Ads4 == null ? "" : Ads4.Alt,
            //    Title4 = Ads4 == null ? "" : Ads4.Title,
            //    Address5 = Ads5 == null ? "" : Ads5.Address,
            //    Alt5 = Ads5 == null ? "" : Ads5.Alt,
            //    Title5 = Ads5 == null ? "" : Ads5.Title,
            //    Address6 = Ads6 == null ? "" : Ads6.Address,
            //    Alt6 = Ads6 == null ? "" : Ads6.Alt,
            //    Title6 = Ads6 == null ? "" : Ads6.Title,
            //};

            //var Result = new Tuple<BannersDto, AdsDto>(banner, Ads);

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
        public async Task<IActionResult> ShowUserProfile(string UserName,int? page)
        {
            string[] registerDate;
            var FindUser = await _DbContext.Users.FirstOrDefaultAsync(p => p.UserName == UserName);

            var CurentPage = (page == null || page == 0) ? 1 : page.Value;
            var PageSize = 6;
            var SkipData = (CurentPage - 1) * PageSize;
            var CountData = await _DbContext.News.Where(p => p.WriterId == FindUser.Id &&  p.Status == StatusName.Publish && p.IsActive == true).CountAsync();
            var ToalPage = (int)Math.Ceiling((double)CountData / PageSize);

            ViewBag.Skip = SkipData;
            ViewBag.Take = PageSize;
            ViewBag.CurentPage = CurentPage;
            ViewBag.ToalPage = ToalPage;

            
            if (FindUser == null)
            {
                return Redirect("NotFound/User");
            }
            else
            {
                registerDate = FindUser.DateRegisterPresian.Split(" ");
            }
            var Result = new ShowUserProfileDto()
            {
                BannerForProfile = FindUser.BannerForProfile,
                AboutMe = FindUser.AboutMe,
                FullName = FindUser.FirstName + " " + FindUser.LastName,
                RegisterDate = registerDate[0],
                UserId = FindUser.Id,
                UserPicture = FindUser.PicAddress,
                UserPictureAlt = FindUser.PicAlt,
                UserPictureTitle =  FindUser.PicTitle,
                UserName = FindUser.UserName
            };
            return View(Result);
        }
        [HttpPost]
        [Route("{UserName}")]
        public async Task<IActionResult> ShowUserProfile(ShowUserProfileDto model)
        {
            
            
            var FindUser = await _DbContext.Users.FirstOrDefaultAsync(p => p.Id == model.UserId);

            var stringPath = "";
            if (ModelState.IsValid)
            {
                if (model.BannerPicFile != null)
                {
                    if (model.BannerPicFile.Length > 1000000)
                    {
                        ModelState.AddModelError("PicAddress", "حجم عکس باید زیر یک مگابایت باشد");
                        return View();
                    }
                    if (model.BannerPicFile.ContentType == "image/png" || model.BannerPicFile.ContentType == "image/jpg" ||
                        model.BannerPicFile.ContentType == "image/jpeg" || model.BannerPicFile.ContentType == "image/gif")
                    {
                        stringPath = $"Media/Users/BannerProfile/" + await _fileUpload.UploadFileAsync(model.BannerPicFile,FindUser.UserName, "Users", "BannerProfile");
                    }
                    else
                    {
                        ModelState.AddModelError("PicAddress", "نوع فایل باید به صورت عکس باشد");
                        return View();
                    }
                }
                FindUser.BannerForProfile = stringPath == "" ? FindUser.BannerForProfile : stringPath;
                FindUser.AboutMe = model.AboutMe;

                _DbContext.Entry(FindUser).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();
                return Redirect($"/{FindUser.UserName}");
            }
            return View(model);
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