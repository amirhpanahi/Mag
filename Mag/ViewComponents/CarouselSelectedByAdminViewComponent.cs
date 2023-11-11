using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Entities;
using Mag.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class CarouselSelectedByAdminViewComponent : ViewComponent 
    {
        private readonly DataBaseContext _dbContext;
        public CarouselSelectedByAdminViewComponent(DataBaseContext dataBaseContex)
        {
            _dbContext = dataBaseContex;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            ViewBag.ParentIdCategories = _dbContext.CategoryTags.Where(x => x.Type == "Category" && x.ParentId == 1 && x.Id != 1).ToList();
            ViewBag.ListUsers = _dbContext.Users.ToList();



            var listNews = _dbContext.News.OrderByDescending(p => p.PublishNewsDate)
                .Where(p => p.Status == StatusName.Publish &&
                    p.IsActive == true &&
                    p.IsSelectBychiefEditor == true).Take(7).ToList().Select(p => new NewsCardDto
            {
                Title = p.Title,
                Slug = p.Slug,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                PublishNewsDatePersianDay = getDay(p.PublishNewsDatePersian),
                PublishNewsDatePersianmonth = getmonth(p.PublishNewsDatePersian),
                PublishNewsDatePersianYear = getYear(p.PublishNewsDatePersian),
                Categories = p.Categories == null ? " " : p.Categories,
                CountOfLike = _dbContext.Likes.Where(x => x.NewsId == p.Id && x.StatusLike == StatusLike.Like).Count(),
                CountOfComment = _dbContext.Comments.Where(x => x.NewsId == p.Id && x.Status == Comment.StatusName.Publish).Count(),
                NewsSummary = p.NewsSummary == null ? " " : p.NewsSummary,
                WriterId = p.WriterId
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


