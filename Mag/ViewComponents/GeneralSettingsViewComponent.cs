using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Dto.Settings;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class GeneralSettingsViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public GeneralSettingsViewComponent(DataBaseContext dataBaseContex)
        {
            _dbContext = dataBaseContex;
        }
        public async Task<IViewComponentResult> InvokeAsync(string SecendPart,string NewsId)
         {
            var FindNews = new News();
            if (NewsId != null)
            {
                var NewsIdInt = Convert.ToInt32(NewsId);
                FindNews = await _dbContext.News.FirstOrDefaultAsync(p => p.Id == NewsIdInt);
            }

            var settings = await _dbContext.Settings.Select(x => new GeneralSettingDto
            {
                FavIconAddress = x.FavIconAddress,
                SiteName = (FindNews.Title == null || FindNews.Title == "") ? x.SiteName + " - " + SecendPart : x.SiteName + " - " + FindNews.Title, 
                SeoDescription = (FindNews.DescriptionSeo == null || FindNews.DescriptionSeo == "") ? x.SeoDescription:FindNews.DescriptionSeo,
            }).FirstAsync();

            return View(settings);
        }
    }
}
