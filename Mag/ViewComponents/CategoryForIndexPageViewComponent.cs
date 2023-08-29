using Mag.Areas.Admin.Models.Dto.Category;
using Mag.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mag.ViewComponents
{
    public class CategoryForIndexPageViewComponent : ViewComponent
    {
        private readonly DataBaseContext _DbContext;
        public CategoryForIndexPageViewComponent(DataBaseContext dataBaseContext)
        {
            _DbContext = dataBaseContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Categories = _DbContext.CategoryTags.Where(x => x.Type == "Category" && x.ParentId == 1 && x.Id!=1).Select(p => new CategoryListDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                PicAddress = p.PicAddress,
                PicAlt = p.PicAlt,
                PicTitle = p.PicTitle,
                Slug = p.Slug,
                
            }).ToList();
            
            return View(Categories);
        }
    }
}
