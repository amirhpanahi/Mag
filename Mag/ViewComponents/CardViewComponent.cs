using Mag.Areas.Admin.Models.Dto.Ads;
using Mag.Areas.Admin.Models.Dto.Comment;
using Mag.Areas.Admin.Models.Dto.News;
using Mag.Areas.Admin.Models.Dto.Tag;
using Mag.Data;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace Mag.ViewComponents
{
    public class CardViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        private readonly UserManager<User> _userManager;
        public CardViewComponent(DataBaseContext dataBaseContext, UserManager<User> userManager)
        {
            _dbContext = dataBaseContext;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(string slug,string UserIdVisitor)
        {
            var listGif = _dbContext.Ads.OrderBy(x=>x.IndexNumber).Where(p => p.Name.Contains("NP")).Select(x => x.Address).ToList();
            var FindNews =await _dbContext.News.FirstOrDefaultAsync(p => p.Slug == slug);
            var FindUser = await _userManager.FindByIdAsync(FindNews.WriterId);
            var ParentIdCategories = _dbContext.CategoryTags.Where(x => x.Type == "Category" && x.ParentId == 1 && x.Id != 1).Select(p => p.Id).ToList();
            var pcName =await _dbContext.CategoryTags.FirstOrDefaultAsync(p => p.Id == GetParentIdCategory(FindNews.Categories, ParentIdCategories));

            var Like = new Like();
            if (UserIdVisitor != null)
                 Like = await _dbContext.Likes.FirstOrDefaultAsync(p => p.NewsId == FindNews.Id && p.UserId == UserIdVisitor);

            var NumberOfLike = await _dbContext.Likes.Where(p=>p.NewsId == FindNews.Id && p.StatusLike==StatusLike.Like).CountAsync();

            var ListComments = _dbContext.Comments.Where(p => p.Status == Comment.StatusName.Publish && p.NewsId == FindNews.Id).Select(p => new CommentListDto
            {
                Id = p.Id,
                NewsId = p.NewsId,
                UserId = p.UserId,
                WriterName = _dbContext.Users.Where(x => x.Id == p.UserId).Select(x => x.FirstName + " " + x.LastName).First(),
                CommentText = p.CommentText,
                RegisterDate = p.RegisterDate,
                RegisterDatePersian = p.RegisterDatePersian,
                ParentId = p.ParentId,
            }).ToList();

            var news = new NewsCardDto
            {
                Id = FindNews.Id,
                Title = FindNews.Title,
                Slug = FindNews.Slug,
                ParentCategory = pcName == null ? "سایر" : pcName.Name,
                ParentCategoryId = pcName == null ? null : pcName.Id,
                ParentCategorySlug = pcName == null ? "سایر" : pcName.Slug,
                IndexImageAddress = FindNews.IndexImageAddress,
                VideoAddress = FindNews.VideoAddress == null ? null : FindNews.VideoAddress,
                IndexImageAddressAlt = FindNews.IndexImageAddressAlt,
                IndexImageAddressTitle = FindNews.IndexImageAddressTitle,
                DescriptionHtmlEditor = FindNews.DescriptionHtmlEditor,
                PublishNewsDatePersianDay = getDay(FindNews.PublishNewsDatePersian),
                PublishNewsDatePersianmonth = getmonth(FindNews.PublishNewsDatePersian),
                PublishNewsDatePersianYear = getYear(FindNews.PublishNewsDatePersian),
                PublishNewsDatePersianTime = getTime(FindNews.PublishNewsDatePersian),
                UserImage = FindUser.PicAddress,
                UserName = FindUser.UserName,
                UserFullName = FindUser.FirstName + " " + FindUser.LastName,
                Comments = ListComments,
                NewsSummary = FindNews.NewsSummary == null ? " " : FindNews.NewsSummary,
                Tags = FindNews.Tags == null ? null : GetTags(FindNews.Tags),
                LikeStatus = Like == null ? "" : Like.StatusLike.ToString(),
                CountOfLike = NumberOfLike,
                Gif1Address = listGif[0] == null ? "": listGif[0],
                Gif2Address = listGif[1] == null ? "": listGif[1],
                Gif3Address = listGif[2] == null ? "": listGif[2],
                Gif4Address = listGif[3] == null ? "": listGif[3],
                Gif5Address = listGif[4] == null ? "": listGif[4],
                Gif6Address = listGif[5] == null ? "": listGif[5],
                Gif7Address = listGif[6] == null ? "": listGif[6],
                Gif8Address = listGif[7] == null ? "": listGif[7],
                Gif9Address = listGif[8] == null ? "": listGif[8],
                Gif10Address =listGif[9] == null ? "": listGif[9],
                Gif11Address = listGif[10] == null ? "" : listGif[10]
            };


            return View(news);
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
        private string getTime(string date)
        {
            var SeprateDayMonth = date.Split(" ");
            var GetDayTime = SeprateDayMonth[1].Split(":");
            return GetDayTime[0] + ":" + GetDayTime[1];
        }
        private int GetParentIdCategory(string Categories, List<int>? ParentidCategories)
        {
            var splitCategories = Categories.Split(",");

            foreach (var CatId in splitCategories)
            {
                if (CatId != "")
                {
                    foreach (var CatParentId in ParentidCategories)
                    {
                        if (Convert.ToInt32(CatId) == CatParentId)
                        {
                            return Convert.ToInt32(CatId);
                        }
                    }
                }
            }
            return 0;
        }
        private List<TgsForEachNews> GetTags(string tags)
        {
            var listTag = new List<TgsForEachNews>();
            var splitTags = tags.Split(",");

            foreach (var Tag in splitTags)
            {
                if (Tag != "")
                {
                    var findtag = _dbContext.CategoryTags.FirstOrDefault(p => p.Id == Convert.ToInt32(Tag));
                    var lista = new TgsForEachNews
                    {
                        Id = findtag.Id,
                        Name = findtag.Name,
                    };
                    listTag.Add(lista);
                }
            }
            return listTag;
        }
    }
}
