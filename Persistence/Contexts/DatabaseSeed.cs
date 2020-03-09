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
                    new Product {Id = 103, Name = "Cordel Der"},
                    new Product {Id = 104, Name = "Cordel Japan"}
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            // If there are no ProductVersions
            if (!context.ProductVersions.Any())
            {
                var productVersions = new List<ProductVersion>
                {
                    new ProductVersion {Id = 100, ProductId = 100, Version = "2.5.4"},
                    new ProductVersion {Id = 101, ProductId = 100, Version = "3.1.1"},
                    new ProductVersion {Id = 102, ProductId = 101, Version = "5.3"},
                    new ProductVersion {Id = 103, ProductId = 102, Version = "4.2"},
                    new ProductVersion {Id = 104, ProductId = 103, Version = "2.4"}
                };
                context.ProductVersions.AddRange(productVersions);
                context.SaveChanges();
            }

            // If there are no Releases
            if (!context.Releases.Any())
            {
                var releases = new List<Release>
                {
                    new Release {Id = 100, ProductVersionId = 100, Title = "Finally Rich", IsPublic = true},
                    new Release {Id = 101, ProductVersionId = 101, Title = "Love Sosa", IsPublic = false},
                    new Release {Id = 102, ProductVersionId = 102, Title = "Chief Keef", IsPublic = true},
                    new Release {Id = 103, ProductVersionId = 103, Title = "2012", IsPublic = false},
                    new Release
                    {
                        Id = 104, ProductVersionId = 103,
                        Title = "Han kunne ikke fordra paprika, men så skjedde dette ", IsPublic = true
                    },
                    new Release {Id = 105, ProductVersionId = 102, Title = "asgsdhjkal", IsPublic = false}
                };
                context.Releases.AddRange(releases);
                context.SaveChanges();
            }

            if (!context.ReleaseNotes.Any())
            {
                const string htmlDummy1 =
                    "<div><div>Fixed the shangala bangala<br></div><div><img src=\"https://dev.azure.com/ReleaseNoteSystem/399f705f-cd58-45f2-becb-f890cb50f774/_apis/wit/attachments/9a3d3458-8831-4eb4-af7d-adf0269c8e32?fileName=image.png\" alt=Image></div><div><br></div><div><ol><li>Lars Skriver Mye</li><li>Lars Skriver Mye</li><li>Lars Skriver Mye</li><li>Lars Skriver Mye</li><li>Lars Skriver Mye</li><li>Lars Skriver MyeLars <b>Skriver</b> MyeLars <i>Skriver Mye</i><br></li></ol></div><span>&#128521;</span><span>&#128515;</span></div><blockquote style=\"margin-top:0px;margin-bottom:0px;\"><blockquote style=\"margin-top:0px;margin-bottom:0px;\"><blockquote style=\"margin-top:0px;margin-bottom:0px;\"><div>fafaas</div></blockquote></blockquote><div>Lars Skriver Mye</div></blockquote><div><br></div><div><br></div><div><h1>Lars Skriver Mye<br></h1></div><div><h2></h2></div>";
                const string htmlDummy2 = "<div>fix house-arrest bug</div>";
                const string htmlDummy3 = "<div>Eliminated all escapists</div>";
                const string htmlDummy4 = "<div>Whoa whoa hey hey hey</div>";
                var releaseNotes = new List<ReleaseNote>
                {
                    new ReleaseNote
                    {
                        WorkItemId = 20, AuthorName = "Ronnay Voudrait", AuthorEmail = "ronnay@natron.no",
                        Title = "Corona i Kina", Ingress = "Det er nå påvist masse corona i Kina",
                        Description = "Ikke bra, sier forskerer", WorkItemDescriptionHtml = htmlDummy1,
                        WorkItemTitle = "Test item please ignore",
                        ClosedDate = new DateTime(2001, 7, 11, 23, 5, 12, 23)
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 21, AuthorName = "Aung San Suu Kyi", AuthorEmail = "aungsansuukyi@natron.no",
                        Title = "Corona i Italia", Ingress = "Nå har Corona kommet til Italia",
                        Description = "Ikke bra, sier forskere",
                        WorkItemDescriptionHtml = htmlDummy2, WorkItemTitle = "Receive Nobel Price",
                        ClosedDate = new DateTime(2012, 6, 16, 16, 8, 24, 44)
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 22, AuthorName = "Liu chi", AuthorEmail = "luichi@natron.rno",
                        Title = "Corona i Norge", Ingress = "Den er i Tromsø", Description = "unlucky, sier forskere",
                        WorkItemDescriptionHtml = htmlDummy3, WorkItemTitle = "Forbid escapism",
                        ClosedDate = new DateTime(2190, 7, 11, 06, 21, 21, 21)
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 24, AuthorName = "Lost Sight", AuthorEmail = "IMLSXYZ9@gmail.com",
                        Title = "Man malds on stream",
                        Ingress = "Le Snek is malding over draft",
                        Description = "This LCK color caster is losing follicles by the second",
                        WorkItemDescriptionHtml = htmlDummy2, WorkItemTitle = "Cut the western bullshit",
                        ClosedDate = new DateTime(1989, 3, 16, 02, 22, 42, 1)
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 20, AuthorName = "Ronnay Voudrait", AuthorEmail = "ronnay@natron.no",
                        Title = "Trump bygger vegg mot Corona", Ingress = "Kina skal betale for veggen",
                        Description = "Det hjelper fint lite, sier forskere",
                        WorkItemDescriptionHtml = htmlDummy4, WorkItemTitle = "Fix issues with the application",
                        ClosedDate = new DateTime(2005, 7, 11, 14, 0, 59, 4)
                    }
                };
                context.ReleaseNotes.AddRange(releaseNotes);
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
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec nunc felis, euismod nec tristique eu, egestas ac risus. Mauris rutrum, nibh malesuada sollicitudin tincidunt, orci augue egestas magna, vitae ultricies risus sem a arcu.",
                        Date = new DateTime(2005, 7, 11, 23, 0, 59, 4)
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

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}