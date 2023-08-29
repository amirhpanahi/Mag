using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mag.Areas.Admin.Models.Dto.User
{
    public class AddUserRoleDto
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
