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

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>().HasKey(ur => new {ur.UserId, ur.RoleId});

            // RELEASE SETUP
            // builder.Entity<Release>().Property(r => r.Id).UseHiLo();
            // builder.Entity<Release>().Property(r => r.IsPublic).HasDefaultValue(true);

            // ARTICLE SETUP
            // builder.Entity<Article>().Property(a => a.Id).UseIdentityAlwaysColumn();
            // builder.Entity<Article>().Property(a => a.IsPublic).HasDefaultValue(true);
            // builder.Entity<Article>().Property(a => a.Date).HasDefaultValue(DateTime.UtcNow);
            // builder.Entity<Article>()
            //     .HasOne(a => a.Release)
            //     .WithOne(r => r.Article)
            //     .HasForeignKey<Release>(r => r.ArticleId);
            //
            // builder.Entity<Article>().HasData
            // (
            //     new Article {Id = 100, Uri = "Article001"}
            // );
        }
    }
}