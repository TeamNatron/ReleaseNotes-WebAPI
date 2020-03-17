using System;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;

namespace ReleaseNotes_WebAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<ReleaseNote> ReleaseNotes { get; set; }
        public DbSet<ProductVersion> ProductVersions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReleaseReleaseNote> ReleaseReleaseNotes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>().HasKey(ur => new {ur.UserId, ur.RoleId});
            builder.Entity<ReleaseNote>().Property(rn => rn.Id).ValueGeneratedOnAdd();
            builder.Entity<Article>().Property(a => a.Date).HasDefaultValue(DateTime.UtcNow);
            builder.Entity<Release>().Property(r => r.Date).HasDefaultValue(DateTime.UtcNow);
            builder.Entity<ReleaseReleaseNote>().HasKey(
                rrn => new {rrn.ReleaseId, rrn.ReleaseNoteId});
        }
    }
}