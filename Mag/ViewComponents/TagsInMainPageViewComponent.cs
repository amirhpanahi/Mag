using Mag.Areas.Admin.Models.Dto.Tag;
using Mag.Data;
using Mag.Models.Dto.Settings;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Mag.ViewComponents
{
    public class TagsInMainPageViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public TagsInMainPageViewComponent(DataBaseContext dataBaseContex)
        {
            _dbContext = dataBaseContex;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Tags = _dbContext.CategoryTags.OrderByDescending(x=>x.Id).Where(x => x.Type == "Tag").Select(x => new TagListDto
            {
                Name = x.Name,
                Id = x.Id,
            }).Take(4).ToList();

            return View(Tags);
        }
    }
}
