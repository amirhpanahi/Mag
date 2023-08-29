using Microsoft.AspNetCore.Identity;

namespace Mag.Models.Entities
{
    public class Role:IdentityRole
    {
        public string Description { get; set; }
    }
}
