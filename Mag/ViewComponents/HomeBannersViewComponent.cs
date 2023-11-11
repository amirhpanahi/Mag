using Mag.Models.Dto.Settings;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class HomeBannersViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string SecendPart, string NewsId)
        {
            return View();
        }
    }
}
