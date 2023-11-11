using Mag.Areas.Admin.Models.Dto.Comment;
using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Dto.Home;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class SidebarMainPageViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public SidebarMainPageViewComponent(DataBaseContext dataBaseContex)
        {
            _dbContext = dataBaseContex;
        }
        public async Task<IViewComponentResult> InvokeAsync() 
        {
            var result = _dbContext.CategoryTags.Where(x => x.IsShowMainPage == true && x.Type == "Category" && x.Id != 1).Select(x => new NavigationForLayoutPageDto
            {
                Name = x.Name,
                Slug = x.Slug,
            }).ToList();

            return View(result);
        }
    }
}
