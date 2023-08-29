using Mag.Areas.Admin.Models.Dto.User;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mag.ViewComponents
{
    public class LayoutUserViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        public LayoutUserViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(string Id)
        {
            var userFind = await _userManager.FindByIdAsync(Id);
            var user = new UserListDto
            {
                Id = userFind.Id,
                FirstName = userFind.FirstName,
                LastName = userFind.LastName,
                Email = userFind.Email,
                PhoneNumber = userFind.PhoneNumber,
                EmailConfirmed = userFind.EmailConfirmed,
                PicAddress = userFind.PicAddress,
                PicAlt = userFind.PicAlt,
                PicTitle = userFind.PicTitle,
                DateRegisterPresian = userFind.DateRegisterPresian,
                LastLoginDatePersian = userFind.LastLoginDatePersian,
                DateRegister = userFind.DateRegister,
                LastLoginDate = userFind.LastLoginDate
            };
            return View(user);
        }
    }
}
