using Mag.Data;
using Mag.Models.Dto.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class LogoAdminPanelViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public LogoAdminPanelViewComponent(DataBaseContext dataBaseContext)
        {
            _dbContext = dataBaseContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _dbContext.Settings.Select(x => new GeneralSettingDto
            {
                LogoAddress = x.LogoAddress
            }).FirstAsync();

            return View(settings);
        }
    }
}
