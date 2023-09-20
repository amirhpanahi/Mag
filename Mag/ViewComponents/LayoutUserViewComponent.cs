using Mag.Areas.Admin.Models.Dto.User;
using Mag.Data;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class LayoutUserViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly DataBaseContext _DbContext;
        public LayoutUserViewComponent(UserManager<User> userManager,DataBaseContext dataBaseContext)
        {
            _userManager = userManager;
            _DbContext = dataBaseContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(string Id)
        {
            var userFind = await _userManager.FindByIdAsync(Id);
            var roleFind = await _DbContext.Roles.Where(p => p.Name == "admin").FirstAsync();
            var userRole = await _DbContext.UserRoles.FirstOrDefaultAsync(p => p.UserId == userFind.Id && p.RoleId == roleFind.Id);
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
                LastLoginDate = userFind.LastLoginDate,

                IsAdmin = userRole == null ? false : true
            };
            return View(user);
        }
    }
}
