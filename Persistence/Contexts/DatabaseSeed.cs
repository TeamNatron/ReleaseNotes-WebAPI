using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Security;

namespace ReleaseNotes_WebAPI.Persistence.Contexts
{
    /// <summary>
    /// EF Core already supports database seeding throught overriding "OnModelCreating", but I decided to create a separate seed class to avoid 
    /// injecting IPasswordHasher into AppDbContext.
    /// To understand how to use database seeding into DbContext classes, check this link: https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
    /// </summary>
    public class DatabaseSeed
    {
        public static void Seed(AppDbContext context, IPasswordHasher passwordHasher)
        {
            context.Database.EnsureCreated();

            // If there are no ProductVersions
            if (!context.ProductVersions.Any())
            {
                Console.WriteLine("Creating new ProductVersions");
                var productVersions = new List<ProductVersion>
                {
                    new ProductVersion {Id = 100},
                    new ProductVersion {Id = 101},
                    new ProductVersion {Id = 102},
                    new ProductVersion {Id = 103}
                };
                
                Console.WriteLine("Added these articles:");
                Console.WriteLine(productVersions);
                context.ProductVersions.AddRange(productVersions);
                context.SaveChanges();
            }
            
            // If there are no Releases
            if (!context.Releases.Any())
            {
                Console.WriteLine("Creating new Releases");
                var releases = new List<Release>
                {
                    new Release {Id = 100,ProductVersionId = 100},
                    new Release {Id = 101,ProductVersionId = 101},
                    new Release {Id = 102,ProductVersionId = 102},
                    new Release {Id = 103,ProductVersionId = 103}
                };
                
                Console.WriteLine("Added these Releases:");
                Console.WriteLine(releases);
                context.Releases.AddRange(releases);
                context.SaveChanges();
            }
            
            // If there are no articles
            if (!context.Articles.Any())
            {
                Console.WriteLine("Creating new Articles");
                var articles = new List<Article>
                {
                    new Article {Id = 100, ReleaseId = 100, Uri = "article001.rtf", Title = "Release 1.2 - AR"},
                    new Article {Id = 101, ReleaseId = 101, Uri = "article002.rtf", Title = "Release 2.2 - VR"},
                    new Article {Id = 102, ReleaseId = 102, Uri = "article003.rtf", Title = "Release 3.2 - Parachute"},
                    new Article {Id = 103, ReleaseId = 103, Uri = "article004.rtf", Title = "Release 4.2 - Bazooka"}
                };
                
                Console.WriteLine("Added these articles:");
                Console.WriteLine(articles);
                context.Articles.AddRange(articles);
                context.SaveChanges();
            }
            
            // If there are no roles
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role {Name = ERole.Common.ToString()},
                    new Role {Name = ERole.Administrator.ToString()}
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            // If there are no users
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User {Email = "admin@ungspiller.no", Password = passwordHasher.HashPassword("12345678")},
                    new User {Email = "common@ungspiller.no", Password = passwordHasher.HashPassword("12345678")}
                };

                users[0].UserRoles.Add(new UserRole
                {
                    RoleId = context.Roles.SingleOrDefault(
                        r => r.Name == ERole.Administrator.ToString()).Id
                });

                users[1].UserRoles.Add(new UserRole
                {
                    RoleId = context.Roles.SingleOrDefault(
                        r => r.Name == ERole.Common.ToString()).Id
                });

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}