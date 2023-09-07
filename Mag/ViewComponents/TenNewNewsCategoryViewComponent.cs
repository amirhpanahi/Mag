using Mag.Areas.Admin.Models.Dto.Comment;
using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace Mag.ViewComponents
{
    public class TenNewNewsCategoryViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public TenNewNewsCategoryViewComponent(DataBaseContext dataBaseContex)
        {
            _dbContext = dataBaseContex;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Pid)
        {
            var cat = _dbContext.CategoryTags.FirstOrDefault(p => p.Id == Pid);

            var listNews = _dbContext.News.OrderByDescending(p => p.PublishNewsDate).Where(p => p.Status == StatusName.Publish
            && ("," + p.Categories + ",").Contains("," + Pid + ",")).Take(10).ToList().Select(p=>new NewsListDto
            {
                Title = p.Title,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                Slug = p.Slug
            }).ToList();

            var RetVal = new Tuple<List<NewsListDto>?,string>(listNews,cat.Name);
            return View(RetVal);
        }
    }
}
