using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Dto.Home;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class NavigationForLayoutPageViewComponent : ViewComponent
    {
        private readonly DataBaseContext _DbContext;
        public NavigationForLayoutPageViewComponent(DataBaseContext dataBaseContext)
        {
            _DbContext = dataBaseContext ;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = _DbContext.CategoryTags.Where(x => x.IsShowMainPage == true && x.Type == "Category" && x.Id != 1).Select(x => new NavigationForLayoutPageDto
            {
                Name = x.Name,
                Slug = x.Slug,
            }).ToList();


            return View(result);
        }
    }
}
