using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Mag.ViewComponents
{
    public class FiveTopNewsForMainPageViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public FiveTopNewsForMainPageViewComponent(DataBaseContext dataBaseContext)
        {
            _dbContext = dataBaseContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = _dbContext.Likes
                                .GroupJoin(_dbContext.News,
                                    like => like.NewsId,
                                    news => news.Id,
                                    (like, news) => new { like, news })
                                .SelectMany(
                                    x => x.news.DefaultIfEmpty(),
                                    (like, news) => new { like, news })
                                .GroupBy(
                                x => new
                                {
                                    x.news.Id,
                                    x.news.Title,
                                    x.news.PublishNewsDatePersian,
                                    x.news.Slug,
                                    x.news.IndexImageAddress,
                                    x.news.IndexImageAddressAlt,
                                    x.news.IndexImageAddressTitle
                                },
                                (key, group) => new
                                {
                                    Id = key.Id,
                                    Title = key.Title,
                                    PublishNewsDatePersian = key.PublishNewsDatePersian,
                                    Slug = key.Slug,
                                    IndexImageAddress = key.IndexImageAddress,
                                    IndexImageAddressAlt = key.IndexImageAddressAlt,
                                    IndexImageAddressTitle = key.IndexImageAddressTitle,
                                    NumberOfLike = group.Count()
                                })
                                .OrderByDescending(x => x.NumberOfLike)
                                .Take(5)
                                .ToList();

            var listNews = result.Select(p => new NewsCardDto
            {
                Title = p.Title,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                Slug = p.Slug,
                PublishNewsDatePersianDay = getDay(p.PublishNewsDatePersian),
                PublishNewsDatePersianmonth = getmonth(p.PublishNewsDatePersian),
                PublishNewsDatePersianYear = getYear(p.PublishNewsDatePersian),
            }).ToList();
            return View(listNews);
        }
        private string getDay(string date)
        {
            var SeprateDayMonth = date.Split(" ");
            var GetDayMonth = SeprateDayMonth[0].Split("/");
            return GetDayMonth[2];
        }
        private string getmonth(string date)
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
        private string getYear(string date)
        {
            var SeprateDayMonth = date.Split(" ");
            var GetDayMonth = SeprateDayMonth[0].Split("/");
            return GetDayMonth[0];
        }
    }
}
