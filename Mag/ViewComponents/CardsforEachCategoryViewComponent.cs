using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class CardsforEachCategoryViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public CardsforEachCategoryViewComponent(DataBaseContext dataBaseContext)
        {
            _dbContext = dataBaseContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(string slug)
        {
            var categoryfind = _dbContext.CategoryTags.FirstOrDefault(p => p.Slug == slug);

            //var a = _dbContext.News.Where(p => p.Categories.Contains(categoryfind.Id.ToString()));
            //var listnews = _dbContext.News.Where(p => p.Status == StatusName.Publish && p.PublishNewsDatePersian != null && (","+ p.Categories + ",").Contains(","+ categoryfind.Id + ",")).Select(p => new NewsCardDto
            var listnews = new List<NewsCardDto>();
            if (slug != null && categoryfind != null)
            {
                listnews = _dbContext.News.Where(p => p.PublishNewsDatePersian != null && ("," + p.Categories + ",").Contains("," + categoryfind.Id + ",")).Select(p => new NewsCardDto
                {
                    Title = p.Title,
                    Slug = p.Slug,
                    IndexImageAddress = p.IndexImageAddress,
                    IndexImageAddressAlt = p.IndexImageAddressAlt,
                    IndexImageAddressTitle = p.IndexImageAddressTitle,
                    PublishNewsDatePersianDay = getDay(p.PublishNewsDatePersian),
                    PublishNewsDatePersianmonth = getmonth(p.PublishNewsDatePersian),
                    CountOfLike = _dbContext.Likes.Where(x => x.NewsId == p.Id && x.StatusLike == StatusLike.Like).Count(),
                    CountOfComment = _dbContext.Comments.Where(x => x.NewsId == p.Id && x.Status == Comment.StatusName.Publish).Count(),
                    NewsSummary = p.NewsSummary == null ? " " : p.NewsSummary,
                }).ToList();
            }
            else
            {
                listnews = _dbContext.News.Where(p => p.PublishNewsDatePersian != null).Select(p => new NewsCardDto
                {
                    Title = p.Title,
                    Slug = p.Slug,
                    IndexImageAddress = p.IndexImageAddress,
                    IndexImageAddressAlt = p.IndexImageAddressAlt,
                    IndexImageAddressTitle = p.IndexImageAddressTitle,
                    PublishNewsDatePersianDay = getDay(p.PublishNewsDatePersian),
                    PublishNewsDatePersianmonth = getmonth(p.PublishNewsDatePersian),
                    NewsSummary = p.NewsSummary == null ? " " : p.NewsSummary,
                }).ToList();
            }
            return View(listnews);
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
    }
}
