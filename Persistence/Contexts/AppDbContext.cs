using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models.Auth;

namespace ReleaseNotes_WebAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>().HasKey(ur => new {ur.UserId, ur.RoleId});
        }
    }
}