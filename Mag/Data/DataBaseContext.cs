using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mag.Data
{
    public class DataBaseContext : IdentityDbContext<User,Role,string>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options): base(options)
        {
        }

        public DbSet<CategoryTag> CategoryTags { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Banners> Banners { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}
