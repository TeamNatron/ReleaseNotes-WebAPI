using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Security;

namespace ReleaseNotes_WebAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        /**
         * Truth table for query filter 
         *
         * 1 0 == 1 -> Entity is public, and user is not admin
         * 0 1 == 1 -> Entity is not public, and user is admin
         * 1 1 == 1 -> Entity is public, and user is admin
         * 0 0 == 0 -> Entity is not public, and user is not admin
         * 
        */
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<ReleaseNote> ReleaseNotes { get; set; }
        public DbSet<ProductVersion> ProductVersions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReleaseReleaseNote> ReleaseReleaseNotes { get; set; }
        public DbSet<AzureInformation> AzureInformations { get; set; }
        public DbSet<MappableField> MappableFields { get; set; }
        public DbSet<ReleaseNoteMapping> ReleaseNoteMappings { get; set; }
        public DbSet<MappableType> MappableTypes { get; set; }

        private bool UserIsAdmin { get; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor accessor) : base(options)
        {
            // By default no user is admin
            UserIsAdmin = false;

            // If the context is setting up for the first time. Don't run
            if (accessor.HttpContext != null)
            {
                UserIsAdmin = ClaimsUtil.CheckIfUserIsAdmin(accessor);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Generate default values
            builder.Entity<Article>().Property(a => a.Date).HasDefaultValue(DateTime.UtcNow);
            builder.Entity<Release>().Property(r => r.Date).HasDefaultValue(DateTime.UtcNow);
            
            // Generate primary key on add
            builder.Entity<AzureInformation>().Property(ai => ai.Id).ValueGeneratedOnAdd();
            builder.Entity<ReleaseNote>().Property(rn => rn.Id).ValueGeneratedOnAdd();
            builder.Entity<ProductVersion>().Property(pv => pv.Id).ValueGeneratedOnAdd();
            builder.Entity<MappableField>().Property(mf => mf.Id).ValueGeneratedOnAdd();
            builder.Entity<MappableType>().Property(mt => mt.Id).ValueGeneratedOnAdd();

            // Generate unique constraint
            //builder.Entity<ReleaseNoteMapping>().HasIndex(mf => mf.MappableFieldId).IsUnique();
            builder.Entity<MappableType>().HasIndex(mt => mt.Name).IsUnique();

            // Generate key types
            builder.Entity<UserRole>().HasKey(ur => new {ur.UserId, ur.RoleId});
            builder.Entity<ReleaseNoteMapping>()
                .HasKey(rnm => new {rnm.MappableFieldId, rnm.MappableTypeId});
            builder.Entity<ReleaseReleaseNote>().HasKey(
                rrn => new {rrn.ReleaseId, rrn.ReleaseNoteId});
            
            // All entities that require extra security for isPublic
            builder.Entity<Release>().HasQueryFilter(
                r => !(!r.IsPublic && !UserIsAdmin));
            builder.Entity<ProductVersion>().HasQueryFilter(
                pr => !(!pr.IsPublic && !UserIsAdmin));
            builder.Entity<ReleaseNote>().HasQueryFilter(
                rn => !(!rn.IsPublic && !UserIsAdmin));
        }
    }
}