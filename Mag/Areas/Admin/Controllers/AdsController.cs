using Mag.Areas.Admin.Models.Dto.Ads;
using Mag.Areas.Admin.Models.Dto.Home;
using Mag.Data;
using Mag.Models.Entities;
using Mag.Services.FileUploadService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Mag.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdsController : Controller
    {
        private readonly DataBaseContext _dbContext;
        private readonly IFileUploadService _fileUpload;
        public AdsController(DataBaseContext dataBaseContext,IFileUploadService fileUploadService)
        {
          _dbContext = dataBaseContext;
          _fileUpload = fileUploadService;
        }
        #region AdsMainPage
        [HttpGet]
        public IActionResult AdsMainPage()
        {
            var Ads1 = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP1");
            var Ads2 = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP2");
            var Ads3 = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP3");
            var Ads4 = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP4");
            var Ads5 = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP5");
            var Ads6 = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP6");

            var Advertisment = new AdsDto
            {
                Address1 = Ads1 == null ? "" : Ads1.Address,
                Description1 = Ads1 == null ? "" : Ads1.Description,
                Alt1 = Ads1 == null ? "" : Ads1.Alt,
                Title1 = Ads1 == null ? "" : Ads1.Title,

                Address2 = Ads2 == null ? "" : Ads2.Address,
                Description2 = Ads2 == null ? "" : Ads2.Description,
                Alt2 = Ads2 == null ? "" : Ads2.Alt,
                Title2 = Ads2 == null ? "" : Ads2.Title,

                Address3 = Ads3 == null ? "" : Ads3.Address,
                Description3 = Ads3 == null ? "" : Ads3.Description,
                Alt3 = Ads3 == null ? "" : Ads3.Alt,
                Title3 = Ads3 == null ? "" : Ads3.Title,

                Address4 = Ads4 == null ? "" : Ads4.Address,
                Description4 = Ads4 == null ? "" : Ads4.Description,
                Alt4 = Ads4 == null ? "" : Ads4.Alt,
                Title4 = Ads4 == null ? "" : Ads4.Title,

                Address5 = Ads5 == null ? "" : Ads5.Address,
                Description5 = Ads5 == null ? "" : Ads5.Description,
                Alt5 = Ads5 == null ? "" : Ads5.Alt,
                Title5 = Ads5 == null ? "" : Ads5.Title,

                Address6 = Ads6 == null ? "" : Ads6.Address,
                Description6 = Ads6 == null ? "" : Ads6.Description,
                Alt6 = Ads6 == null ? "" : Ads6.Alt,
                Title6 = Ads6 == null ? "" : Ads6.Title,

            };
            return View(Advertisment);
        }
        [HttpPost]
        public async Task<IActionResult> AdsMainPage(AdsDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Gif1 != null)
                {
                    var findGif = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP1");
                    if (findGif == null)
                    {
                        await SaveGif(model.Gif1, "MP1");
                    }
                    else
                    {
                        await EditGif(model.Gif1, "MP1");
                    }
                }
                if (model.Alt1 != null || model.Title1 != null || model.Description1 != null)
                {
                    await EditGifAltTitleDescription("MP1", model.Alt1, model.Title1,model.Description1);
                }
                if (model.Gif2 != null)
                {
                    var findGif = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP2");
                    if (findGif == null)
                    {
                        await SaveGif(model.Gif2, "MP2");
                    }
                    else
                    {
                        await EditGif(model.Gif2, "MP2");
                    }
                }
                if (model.Alt2 != null || model.Title2 != null || model.Description2 != null)
                {
                    await EditGifAltTitleDescription("MP2", model.Alt2, model.Title2, model.Description2);
                }
                if (model.Gif3 != null)
                {
                    var findGif = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP3");
                    if (findGif == null)
                    {
                        await SaveGif(model.Gif3, "MP3");
                    }
                    else
                    {
                        await EditGif(model.Gif3, "MP3");
                    }
                }
                if (model.Alt3 != null || model.Title3 != null || model.Description3 != null)
                {
                    await EditGifAltTitleDescription("MP3", model.Alt3, model.Title3, model.Description3);
                }
                if (model.Gif4 != null)
                {
                    var findGif = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP4");
                    if (findGif == null)
                    {
                        await SaveGif(model.Gif4, "MP4");
                    }
                    else
                    {
                        await EditGif(model.Gif4, "MP4");
                    }
                }
                if (model.Alt4 != null || model.Title4 != null || model.Description4 != null)
                {
                    await EditGifAltTitleDescription("MP4", model.Alt4, model.Title4, model.Description4);
                }
                if (model.Gif5 != null)
                {
                    var findGif = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP5");
                    if (findGif == null)
                    {
                        await SaveGif(model.Gif5, "MP5");
                    }
                    else
                    {
                        await EditGif(model.Gif5, "MP5");
                    }
                }
                if (model.Alt5 != null || model.Title5 != null || model.Description5 != null)
                {
                    await EditGifAltTitleDescription("MP5", model.Alt5, model.Title5, model.Description5);
                }
                if (model.Gif6 != null)
                {
                    var findGif = _dbContext.Ads.FirstOrDefault(p => p.Name == "MP6");
                    if (findGif == null)
                    {
                        await SaveGif(model.Gif6, "MP6");
                    }
                    else
                    {
                        await EditGif(model.Gif6, "MP6");
                    }
                }
                if (model.Alt6 != null || model.Title6 != null || model.Description6 != null)
                {
                    await EditGifAltTitleDescription("MP6", model.Alt6, model.Title6, model.Description6);
                }

                return Redirect("/Admin/Ads/AdsMainPage");
            }
            return View();
        }
        #endregion

        #region AdsNewsPage
        public IActionResult AdsNewsPage()
        {
            var Ads1 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP1");
            var Ads2 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP2");
            var Ads3 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP3");
            var Ads4 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP4");
            var Ads5 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP5");
            var Ads6 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP6");
            var Ads7 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP7");
            var Ads8 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP8");
            var Ads9 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP9");
            var Ads10 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP10");
            var Ads11 = _dbContext.Ads.FirstOrDefault(p => p.Name == "NP11");

            var Advertisment = new AdsNewsPageDto
            {
                Address1 = Ads1 == null ? "" : Ads1.Address,
                Address2 = Ads2 == null ? "" : Ads2.Address,
                Address3 = Ads3 == null ? "" : Ads3.Address,
                Address4 = Ads4 == null ? "" : Ads4.Address,
                Address5 = Ads5 == null ? "" : Ads5.Address,
                Address6 = Ads6 == null ? "" : Ads6.Address,
                Address7 = Ads7 == null ? "" : Ads7.Address,
                Address8 = Ads8 == null ? "" : Ads8.Address,
                Address9 = Ads9 == null ? "" : Ads9.Address,
                Address10 = Ads10 == null ? "" : Ads10.Address,
                Address11 = Ads11 == null ? "" : Ads11.Address,
            };

            return View(Advertisment);
        }
        [HttpPost]
        public async Task<IActionResult> AdsNewsPage(AdsNewsPageDto model)
        {
            var findAds = await _dbContext.Ads.FirstOrDefaultAsync(p => p.Name == model.Name);
            if (findAds == null)
            {
                if (ModelState.IsValid)
                {
                    await SaveGifNewsPage(model.Gif, model.Name, model.IndexNumber, model.Alt, model.Title, model.Description);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await EditGifNewsPage(model.Gif, findAds, findAds.Address, model.Name,model.IndexNumber, model.Alt, model.Title, model.Description);
                }
            }
            return Redirect("/Admin/Ads/AdsNewsPage");
        }
        #endregion


        public async Task<bool> SaveGif(IFormFile file, string Name)
        {
            var stringLogoPath = $"Media/Ads/" + await _fileUpload.UploadFileAsync(file, Name, "Ads");
            var ads = new Ads
            {
                Name = Name,
                Address = stringLogoPath,
            };

            await _dbContext.Ads.AddAsync(ads);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditGif(IFormFile file, string Name)
        {
            var findAds = _dbContext.Ads.FirstOrDefault(p => p.Name == Name);
            var stringLogoPath = "";

            if (findAds != null)
                stringLogoPath = $"Media/Ads/" + await _fileUpload.UploadFileAsync(file, Name, "Ads");

            findAds.Address = stringLogoPath;

            _dbContext.Entry(findAds).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditGifAltTitleDescription(string Name, string Alt, string Title,string Description)
        {
            var findAds = _dbContext.Ads.FirstOrDefault(p => p.Name == Name);

            findAds.Alt = Alt;
            findAds.Title = Title;
            findAds.Description =Description;

            _dbContext.Entry(findAds).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SaveGifNewsPage(IFormFile file, string Name, int? IndexNumber, string? Alt = "", string? Title = "", string? Description = "")
        {
            var stringLogoPath = $"Media/Ads/" + await _fileUpload.UploadFileAsync(file, Name, "Ads");
            var ads = new Ads
            {
                Name = Name,
                Address = stringLogoPath,
                Alt = Alt,
                Title = Title,
                Description = Description,
                IndexNumber = IndexNumber
            };

            await _dbContext.Ads.AddAsync(ads);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditGifNewsPage(IFormFile file,Ads Model, string AddressGif, string Name,int? IndexNumber, string? Alt = "", string? Title = "", string? Description = "")
        {
            var stringGifPath = "";
            if (file != null)
            {
                stringGifPath = $"Media/Ads/" + await _fileUpload.UploadFileAsync(file, Name, "Ads");
            }

            Model.Name = Name;
            Model.IndexNumber = IndexNumber;
            Model.Address = file == null ? AddressGif:stringGifPath;
            Model.Alt = Alt == null ?Model.Alt:Alt;
            Model.Title = Title == null ? Model.Title : Title;
            Model.Description = Description == null ? Model.Description : Description;


            _dbContext.Entry(Model).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }

}
