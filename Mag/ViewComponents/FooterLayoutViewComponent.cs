using Mag.Data;
using Mag.Models.Dto.Settings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mag.ViewComponents
{
    public class FooterLayoutViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public FooterLayoutViewComponent(DataBaseContext dataBaseContext)
        {
            _dbContext = dataBaseContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FindSetting = _dbContext.Settings.First();

            var Socials = new Dictionary<string,string>();
            if (FindSetting.SocialMedia != null)
            {
                var splitSocials = FindSetting.SocialMedia.Split(",");
                foreach (var item in splitSocials)
                {
                    if (item != "")
                    {
                        var SocialName = item.Split("=");
                        Socials.Add(SocialName[0], SocialName[1]);  
                    }
                }
            }

            var Setting = new SettingFotterDto
            {
                TextForFooter = FindSetting.TextForFooter,
                CopyrightText = FindSetting.CopyrightText,
                FooterMenu = JsonConvert.DeserializeObject<List<string>>(FindSetting.FooterMenu),
                SocialMedia = Socials
            };
            return View(Setting);
        }
    }
}
