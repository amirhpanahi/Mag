using Mag.Areas.Admin.Models.Dto.Home;
using Mag.Areas.Admin.Models.Dto.Settings;
using Mag.Data;
using Mag.Models.Entities;
using Mag.Services.FileUploadService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Mag.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IFileUploadService _fileUpload;
        private readonly DataBaseContext _DbContext;
        public HomeController(IFileUploadService fileUploadService, DataBaseContext dataBaseContext)
        {
            _fileUpload = fileUploadService;
            _DbContext = dataBaseContext;
        }

        [Route("/admin/index")]
        public IActionResult Index()
        {
            var Drafts = _DbContext.News.Where(p => p.Status == StatusName.Draft).ToList().Count();
            var Publish = _DbContext.News.Where(p => p.Status == StatusName.Publish).ToList().Count();
            var Delete = _DbContext.News.Where(p => p.Status == StatusName.Delete).ToList().Count();
            var Reject = _DbContext.News.Where(p => p.Status == StatusName.RejectedByAdmin).ToList().Count();
            var Wating = _DbContext.News.Where(p => p.Status == StatusName.WaitingForConfirm).ToList().Count();
            var all = _DbContext.News.ToList().Count();
            var home = new IndexPageDto
            {
                NumberOfDraft = Drafts,
                NumberOfPublish = Publish,
                NumberOfDelete = Delete,
                NumberOfReject = Reject,
                NumberOfWateforConfirm = Wating,
                PercentOfWatit = Wating == 0 ? 0 : (int)Math.Floor((decimal)(Wating * 100) / all)
            };
            return View(home);
        }

        #region Settings
        [Route("/admin/Settings")]
        [HttpGet]
        public IActionResult Settings()
        {
            var findSettings = _DbContext.Settings.ToList().FirstOrDefault();
            if (findSettings == null)
            {
                return View();
            }
            else
            {
                var Social = new List<string>();
                var Socialdic = new Dictionary<string, string>();
                var splitSocialMedia = findSettings.SocialMedia.Split(",");
                foreach (var item in splitSocialMedia)
                {
                    if (item != "")
                    {
                        var socialparts = item.Split("=");
                        Socialdic.Add(socialparts[0], socialparts[1]);
                        Social.Add(item);
                    }
                }

                var NewSettings = new SettingsDto
                {
                    Id = findSettings.Id,
                    SiteName = findSettings.SiteName,
                    Title = findSettings.Title,
                    LogoAddress = findSettings.LogoAddress,
                    FavIconAddress = findSettings.FavIconAddress,
                    SeoDescription = findSettings.SeoDescription,
                    KeyWords = findSettings.KeyWords,
                    ScriptHeader = findSettings.ScriptHeader,
                    TextForFooter = findSettings.TextForFooter,
                    FooterMenu = findSettings.FooterMenu,
                    Permissions = findSettings.Permissions,
                    CopyrightText = findSettings.CopyrightText,
                    ScriptFooter = findSettings.ScriptFooter,
                };
                foreach (var item in Socialdic.Keys)
                {
                    if (item == "instagram")
                    {
                        NewSettings.Instagram = true;
                        foreach (var items in Socialdic)
                        {
                            if (items.Key == "instagram")
                            {
                                NewSettings.InstagramLink = items.Value;
                            }
                        }
                    }
                    if (item == "twitter")
                    {
                        NewSettings.Twitter = true;
                        foreach (var items in Socialdic)
                        {
                            if (items.Key == "twitter")
                            {
                                NewSettings.TwitterLink = items.Value;
                            }
                        }
                    }
                    if (item == "facebook")
                    {
                        NewSettings.Facebook = true;
                        foreach (var items in Socialdic)
                        {
                            if (items.Key == "facebook")
                            {
                                NewSettings.FacebookLink = items.Value;
                            }
                        }
                    }
                    if (item == "whatsapp")
                    {
                        NewSettings.Whatsapp = true;
                        foreach (var items in Socialdic)
                        {
                            if (items.Key == "whatsapp")
                            {
                                NewSettings.WhatsappLink = items.Value;
                            }
                        }
                    }
                    if (item == "telegram")
                    {
                        NewSettings.Telegram = true;
                        foreach (var items in Socialdic)
                        {
                            if (items.Key == "telegram")
                            {
                                NewSettings.TelegramLink = items.Value;
                            }
                        }
                    }
                    if (item == "youtube")
                    {
                        NewSettings.Youtube = true;
                        foreach (var items in Socialdic)
                        {
                            if (items.Key == "youtube")
                            {
                                NewSettings.YoutubeLink = items.Value;
                            }
                        }
                    }
                    if (item == "linkedin")
                    {
                        NewSettings.Linkedin = true;
                        foreach (var items in Socialdic)
                        {
                            if (items.Key == "linkedin")
                            {
                                NewSettings.LinkedinLink = items.Value;
                            }
                        }
                    }
                    if (item == "gmail")
                    {
                        NewSettings.Gmail = true;
                        foreach (var items in Socialdic)
                        {
                            if (items.Key == "gmail")
                            {
                                NewSettings.GmailLink = items.Value;
                            }
                        }
                    }
                }
                return View(NewSettings);
            }
        }
        [Route("/admin/Settings")]
        [HttpPost]
        public async Task<IActionResult> Settings(SettingsDto model)
        {
            var findsetting = new Settings();
            if (model.Id != 0)
            {
                findsetting = _DbContext.Settings.FirstOrDefault(P => P.Id == model.Id);
            }
            var SocialMedias = new StringBuilder();

            if (model.Instagram)
                SocialMedias.Append($"instagram={model.InstagramLink},");
            if (model.Twitter)
                SocialMedias.Append($"twitter={model.TwitterLink},");
            if (model.Facebook)
                SocialMedias.Append($"facebook={model.FacebookLink},");
            if (model.Whatsapp)
                SocialMedias.Append($"whatsapp={model.WhatsappLink},");
            if (model.Telegram)
                SocialMedias.Append($"telegram={model.TelegramLink},");
            if (model.Youtube)
                SocialMedias.Append($"youtube={model.YoutubeLink},");
            if (model.Linkedin)
                SocialMedias.Append($"linkedin={model.LinkedinLink},");
            if (model.Gmail)
                SocialMedias.Append($"gmail={model.GmailLink},");

            var stringLogoPath = "";
            if (model.LogoFile != null)
            {
                if (model.LogoFile.Length > 5242848)
                {
                    ModelState.AddModelError("LogoAddress", "حجم عکس باید زیر پنج مگابایت باشد");
                    return View();
                }
                if (model.LogoFile.ContentType == "image/png" || model.LogoFile.ContentType == "image/jpg" ||
                    model.LogoFile.ContentType == "image/jpeg" || model.LogoFile.ContentType == "image/gif")
                {
                    stringLogoPath = $"Media/Logo/" + await _fileUpload.UploadFileAsync(model.LogoFile, model.SiteName, "Logo");
                }
                else
                {
                    ModelState.AddModelError("LogoAddress", "نوع فایل باید به صورت عکس باشد");
                    return View();
                }
            }
            else
                stringLogoPath = null;

            var stringIconPath = "";
            if (model.FavIconFile != null)
            {
                if (model.FavIconFile.Length > 5242848)
                {
                    ModelState.AddModelError("FavIconAddress", "حجم آیکون باید زیر پنج مگابایت باشد");
                    return View();
                }
                if (model.FavIconFile.ContentType == "image/x-icon" || model.FavIconFile.ContentType == "image/png" ||
                    model.FavIconFile.ContentType == "image/jpg" || model.FavIconFile.ContentType == "image/jpeg")
                {
                    stringIconPath = $"Media/Icon/" + await _fileUpload.UploadFileAsync(model.FavIconFile, model.SiteName, "Icon");
                }
                else
                {
                    ModelState.AddModelError("FavIconAddress", "نوع فایل باید به صورت آیکون باشد");
                    return View();
                }
            }
            else
                stringIconPath = null;

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var NewSettings = new Settings
                    {
                        SiteName = model.SiteName,
                        Title = model.Title,
                        LogoAddress = model.LogoFile != null ? stringLogoPath : model.LogoAddress,
                        FavIconAddress = model.FavIconFile != null ? stringIconPath : model.FavIconAddress,
                        SeoDescription = model.SeoDescription,
                        KeyWords = model.KeyWords,
                        ScriptHeader = model.ScriptHeader,
                        TextForFooter = model.TextForFooter,
                        FooterMenu = model.FooterMenu,
                        Permissions = model.Permissions,
                        CopyrightText = model.CopyrightText,
                        ScriptFooter = model.ScriptFooter,
                        SocialMedia = SocialMedias.ToString(),
                    };

                    await _DbContext.Settings.AddAsync(NewSettings);
                    await _DbContext.SaveChangesAsync();
                }
                else
                {
                    findsetting.SiteName = model.SiteName;
                    findsetting.Title = model.Title;
                    findsetting.LogoAddress = model.LogoFile == null ? findsetting.LogoAddress : stringLogoPath;
                    findsetting.FavIconAddress = model.FavIconFile == null ? findsetting.FavIconAddress : stringIconPath;
                    findsetting.SeoDescription = model.SeoDescription;
                    findsetting.KeyWords = model.KeyWords;
                    findsetting.ScriptHeader = model.ScriptHeader;
                    findsetting.TextForFooter = model.TextForFooter;
                    findsetting.FooterMenu = model.FooterMenu;
                    findsetting.Permissions = model.Permissions;
                    findsetting.CopyrightText = model.CopyrightText;
                    findsetting.ScriptFooter = model.ScriptFooter;
                    findsetting.SocialMedia = SocialMedias.ToString();

                    _DbContext.Entry(findsetting).State = EntityState.Modified;
                    await _DbContext.SaveChangesAsync();
                }


                return RedirectToAction("Index", "home", new { Areas = "Admin" });
            }
            return View(model);
        }

        #endregion

        #region NewsBanners
        [Route("/admin/NewsBanners")]
        [HttpGet]
        public IActionResult Banners()
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
        [Route("/admin/NewsBanners")]
        [HttpPost]
        public async Task<IActionResult> Banners(BannersDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Banner1 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == model.Banner1.Name);
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner1, model.Banner1.Name);
                    }else
                    {
                        await EditBaner(model.Banner1,model.Banner1.Name);
                    }
                }
                if (model.Banner1PhotoLink != null || model.Banner1PhotoText != null)
                {
                    await EditBanerTextLink("Banner1", model.Banner1PhotoText,model.Banner1PhotoLink);
                }
                if (model.Banner2 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == model.Banner2.Name);
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner2, model.Banner2.Name);
                    }
                    else
                    {
                        await EditBaner(model.Banner2, model.Banner2.Name);
                    }
                }
                if (model.Banner2PhotoLink != null || model.Banner2PhotoText != null)
                {
                    await EditBanerTextLink("Banner2", model.Banner2PhotoText, model.Banner2PhotoLink);
                }
                if (model.Banner3 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == model.Banner3.Name);
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner3, model.Banner3.Name);
                    }
                    else
                    {
                        await EditBaner(model.Banner3, model.Banner3.Name);
                    }
                }
                if (model.Banner3PhotoLink != null || model.Banner3PhotoText != null)
                {
                    await EditBanerTextLink("Banner3", model.Banner3PhotoText, model.Banner3PhotoLink);
                }
                if (model.Banner4 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == model.Banner4.Name);
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner4, model.Banner4.Name);
                    }
                    else
                    {
                        await EditBaner(model.Banner4, model.Banner4.Name);
                    }
                }
                if (model.Banner4PhotoLink != null || model.Banner4PhotoText != null)
                {
                    await EditBanerTextLink("Banner4", model.Banner4PhotoText, model.Banner4PhotoLink);
                }
                if (model.Banner5 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == model.Banner5.Name);
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner5, model.Banner5.Name);
                    }
                    else
                    {
                        await EditBaner(model.Banner5, model.Banner5.Name);
                    }
                }
                if (model.Banner5PhotoLink != null || model.Banner5PhotoText != null)
                {
                    await EditBanerTextLink("Banner5", model.Banner5PhotoText, model.Banner5PhotoLink);
                }
                if (model.Banner6 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == model.Banner6.Name);
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner6, model.Banner6.Name);
                    }
                    else
                    {
                        await EditBaner(model.Banner6, model.Banner6.Name);
                    }
                }
                if (model.Banner6PhotoLink != null || model.Banner6PhotoText != null)
                {
                    await EditBanerTextLink("Banner6", model.Banner6PhotoText, model.Banner6PhotoLink);
                }
                if (model.Banner7 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == model.Banner7.Name);
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner7, model.Banner7.Name);
                    }
                    else
                    {
                        await EditBaner(model.Banner7, model.Banner7.Name);
                    }
                }
                if (model.Banner7PhotoLink != null || model.Banner7PhotoText != null)
                {
                    await EditBanerTextLink("Banner7", model.Banner7PhotoText, model.Banner7PhotoLink);
                }

                return Redirect("/Admin/Banners");
            }
            return View();
        }

        #endregion

        #region MainBanners
        [Route("/admin/MainBanners")]
        [HttpGet]
        public IActionResult MainBanners()
        {
            var baner1 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner1");
            var baner2 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner2");
            var baner3 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner3");
            var baner4 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner4");
            var baner5 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner5");
            var baner6 = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner6");

            var banner = new BannersDto
            {
                Banner1Address = baner1 == null ? "" : baner1.BannerAddress,
                Banner1PhotoText = baner1 == null ? "" : baner1.PhotoText,
                Banner1PhotoLink = baner1 == null ? "" : baner1.PhotoLink,
                Banner2Address = baner2 == null ? "" : baner2.BannerAddress,
                Banner2PhotoText = baner2 == null ? "" : baner2.PhotoText,
                Banner2PhotoLink = baner2 == null ? "" : baner2.PhotoLink,
                Banner3Address = baner3 == null ? "" : baner3.BannerAddress,
                Banner3PhotoText = baner3 == null ? "" : baner3.PhotoText,
                Banner3PhotoLink = baner3 == null ? "" : baner3.PhotoLink,
                Banner4Address = baner4 == null ? "" : baner4.BannerAddress,
                Banner4PhotoText = baner4 == null ? "" : baner4.PhotoText,
                Banner4PhotoLink = baner4 == null ? "" : baner4.PhotoLink,
                Banner5Address = baner5 == null ? "" : baner5.BannerAddress,
                Banner5PhotoText = baner5 == null ? "" : baner5.PhotoText,
                Banner5PhotoLink = baner5 == null ? "" : baner5.PhotoLink,
                Banner6Address = baner6 == null ? "" : baner6.BannerAddress,
                Banner6PhotoText = baner6 == null ? "" : baner6.PhotoText,
                Banner6PhotoLink = baner6 == null ? "" : baner6.PhotoLink,
            };
            return View(banner);
        }
        [Route("/admin/MainBanners")]
        [HttpPost]
        public async Task<IActionResult> MainBanners(BannersDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Banner1 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner1");
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner1, "MainBanner1");
                    }
                    else
                    {
                        await EditBaner(model.Banner1, "MainBanner1");
                    }
                }
                if (model.Banner1PhotoLink != null || model.Banner1PhotoText != null)
                {
                    await EditBanerTextLink("MainBanner1", model.Banner1PhotoText, model.Banner1PhotoLink);
                }
                if (model.Banner2 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner2");
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner2, "MainBanner2");
                    }
                    else
                    {
                        await EditBaner(model.Banner2, "MainBanner2");
                    }
                }
                if (model.Banner2PhotoLink != null || model.Banner2PhotoText != null)
                {
                    await EditBanerTextLink("MainBanner2", model.Banner2PhotoText, model.Banner2PhotoLink);
                }
                if (model.Banner3 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner3");
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner3, "MainBanner3");
                    }
                    else
                    {
                        await EditBaner(model.Banner3, "MainBanner3");
                    }
                }
                if (model.Banner3PhotoLink != null || model.Banner3PhotoText != null)
                {
                    await EditBanerTextLink("MainBanner3", model.Banner3PhotoText, model.Banner3PhotoLink);
                }
                if (model.Banner4 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner4");
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner4, "MainBanner4");
                    }
                    else
                    {
                        await EditBaner(model.Banner4, "MainBanner4");
                    }
                }
                if (model.Banner4PhotoLink != null || model.Banner4PhotoText != null)
                {
                    await EditBanerTextLink("MainBanner4", model.Banner4PhotoText, model.Banner4PhotoLink);
                }
                if (model.Banner5 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner5");
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner5, "MainBanner5");
                    }
                    else
                    {
                        await EditBaner(model.Banner5, "MainBanner5");
                    }
                }
                if (model.Banner5PhotoLink != null || model.Banner5PhotoText != null)
                {
                    await EditBanerTextLink("MainBanner5", model.Banner5PhotoText, model.Banner5PhotoLink);
                }
                if (model.Banner6 != null)
                {
                    var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == "MainBanner6");
                    if (findBanner == null)
                    {
                        await SaveBaner(model.Banner6, "MainBanner6");
                    }
                    else
                    {
                        await EditBaner(model.Banner6, "MainBanner6");
                    }
                }
                if (model.Banner6PhotoLink != null || model.Banner6PhotoText != null)
                {
                    await EditBanerTextLink("MainBanner6", model.Banner6PhotoText, model.Banner6PhotoLink);
                }

                return Redirect("/Admin/MainBanners");
            }
            return View();
        }

        #endregion

        public async Task<bool> SaveBaner(IFormFile file, string Name)
        {
            var stringLogoPath = $"Media/Banners/" + await _fileUpload.UploadFileAsync(file, Name, "Banners");
            var baner = new Banners
            {
                BannerName = Name,
                BannerAddress = stringLogoPath,
            };

            await _DbContext.Banners.AddAsync(baner);
            await _DbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditBaner(IFormFile file, string Name)
        {
            var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == Name);
            var stringLogoPath = "";

            if (findBanner != null)
                stringLogoPath = $"Media/Banners/" + await _fileUpload.UploadFileAsync(file, Name, "Banners");

            findBanner.BannerAddress = stringLogoPath;

            _DbContext.Entry(findBanner).State = EntityState.Modified;
            await _DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditBanerTextLink(string Name, string PhotoText, string PhotoLink)
        {
            var findBanner = _DbContext.Banners.FirstOrDefault(p => p.BannerName == Name);

            findBanner.PhotoText = PhotoText;
            findBanner.PhotoLink = PhotoLink;

            _DbContext.Entry(findBanner).State = EntityState.Modified;
            await _DbContext.SaveChangesAsync();
            return true;
        }

    }
}
