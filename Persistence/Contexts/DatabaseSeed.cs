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

            // If there are no Products
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product {Id = 100, Name = "Cordel Inne"},
                    new Product {Id = 101, Name = "Cordel Ute"},
                    new Product {Id = 102, Name = "Cordel Her"},
                    new Product {Id = 103, Name = "Cordel Der"}
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
            
            // If there are no ProductVersions
            if (!context.ProductVersions.Any())
            {
                var productVersions = new List<ProductVersion>
                {
                    new ProductVersion {Id = 100, ProductId = 100},
                    new ProductVersion {Id = 101, ProductId = 101},
                    new ProductVersion {Id = 102, ProductId = 102},
                    new ProductVersion {Id = 103, ProductId = 103}
                };
                context.ProductVersions.AddRange(productVersions);
                context.SaveChanges();
            }

            // If there are no Releases
            if (!context.Releases.Any())
            {
                var releases = new List<Release>
                {
                    new Release {Id = 100, ProductVersionId = 100},
                    new Release {Id = 101, ProductVersionId = 101},
                    new Release {Id = 102, ProductVersionId = 102},
                    new Release {Id = 103, ProductVersionId = 103}
                };
                context.Releases.AddRange(releases);
                context.SaveChanges();
            }

            // If there are no articles
            if (!context.Articles.Any())
            {
                var articles = new List<Article>
                {
                    new Article
                    {
                        Id = 100, ReleaseId = 100, Uri = "article001.rtf", Title = "Release 1.2 - AR",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec nunc felis, euismod nec tristique eu, egestas ac risus. Mauris rutrum, nibh malesuada sollicitudin tincidunt, orci augue egestas magna, vitae ultricies risus sem a arcu."
                    },
                    new Article
                    {
                        Id = 101, ReleaseId = 101, Uri = "article002.rtf", Title = "Release 2.2 - VR",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec nunc felis, euismod nec tristique eu, egestas ac risus. Mauris rutrum, nibh malesuada sollicitudin tincidunt, orci augue egestas magna, vitae ultricies risus sem a arcu."
                    },
                    new Article
                    {
                        Id = 102, ReleaseId = 102, Uri = "article003.rtf", Title = "Release 3.2 - Parachute",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec nunc felis, euismod nec tristique eu, egestas ac risus. Mauris rutrum, nibh malesuada sollicitudin tincidunt, orci augue egestas magna, vitae ultricies risus sem a arcu."
                    },
                    new Article
                    {
                        Id = 103, ReleaseId = 103, Uri = "article004.rtf", Title = "Release 4.2 - Bazooka",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec nunc felis, euismod nec tristique eu, egestas ac risus. Mauris rutrum, nibh malesuada sollicitudin tincidunt, orci augue egestas magna, vitae ultricies risus sem a arcu."
                    }
                };
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