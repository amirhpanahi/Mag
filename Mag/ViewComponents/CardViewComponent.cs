using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mag.ViewComponents
{
    public class CardViewComponent : ViewComponent 
    {
        private readonly DataBaseContext _dbContext;
        private readonly UserManager<User> _userManager;
        public CardViewComponent(DataBaseContext dataBaseContext,UserManager<User> userManager)
        {
            _dbContext = dataBaseContext;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(string slug)
        {
            var FindNews = _dbContext.News.FirstOrDefault(p => p.Slug == slug);
            var FindUser = await _userManager.FindByIdAsync(FindNews.WriterId);

            var news = new NewsCardDto
            {
                Title = FindNews.Title,
                Slug = FindNews.Slug,
                IndexImageAddress = FindNews.IndexImageAddress,
                IndexImageAddressAlt = FindNews.IndexImageAddressAlt,
                IndexImageAddressTitle = FindNews.IndexImageAddressTitle,
                DescriptionHtmlEditor = FindNews.DescriptionHtmlEditor,
                PublishNewsDatePersianDay = getDay(FindNews.PublishNewsDatePersian),
                PublishNewsDatePersianmonth = getmonth(FindNews.PublishNewsDatePersian),
                PublishNewsDatePersianYear = getYear(FindNews.PublishNewsDatePersian),
                PublishNewsDatePersianTime = getTime(FindNews.PublishNewsDatePersian),
                UserImage = FindUser.PicAddress,
                UserFullName = FindUser.FirstName +" "+ FindUser.LastName
            };

            return View(news);
        }
        static string getDay(string date)
        {
            var SeprateDayMonth = date.Split(" ");
            var GetDayMonth = SeprateDayMonth[0].Split("/");
            return GetDayMonth[2];
        }
        static string getmonth(string date)
        {
            var SeprateDayMonth = date.Split(" ");
            var GetDayMonth = SeprateDayMonth[0].Split("/");
            var month = GetDayMonth[1];
            var Retmonth = "";
            switch (month)
            {
                case "1":
                    Retmonth = "فروردین";
                    break;
                case "2":
                    Retmonth = "اردیبهشت";
                    break;
                case "3":
                    Retmonth = "خرداد";
                    break;
                case "4":
                    Retmonth = "تیر";
                    break;
                case "5":
                    Retmonth = "مرداد";
                    break;
                case "6":
                    Retmonth = "شهریور";
                    break;
                case "7":
                    Retmonth = "مهر";
                    break;
                case "8":
                    Retmonth = "آبان";
                    break;
                case "9":
                    Retmonth = "آذر";
                    break;
                case "10":
                    Retmonth = "دی";
                    break;
                case "11":
                    Retmonth = "بهمن";
                    break;
                case "12":
                    Retmonth = "اسفند";
                    break;
            }
            return Retmonth;
        }
        static string getYear(string date)
        {
            var SeprateDayMonth = date.Split(" ");
            var GetDayMonth = SeprateDayMonth[0].Split("/");
            return GetDayMonth[0];
        }
        static string getTime(string date)
        {
            var SeprateDayMonth = date.Split(" ");
            var GetDayTime = SeprateDayMonth[1].Split(":");
            return GetDayTime[0]+":"+GetDayTime[1];
        }
    }
}
