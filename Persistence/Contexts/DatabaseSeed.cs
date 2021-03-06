using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Security;

namespace ReleaseNotes_WebAPI.Persistence.Contexts
{
    /// <summary>
    /// EF Core already supports database seeding through overriding "OnModelCreating", but I decided to create a separate seed class to avoid 
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
                    new ProductVersion {Id = 100, ProductId = 100, Version = "1.0", IsPublic = true},
                    new ProductVersion {Id = 101, ProductId = 100, Version = "2.0"},
                    new ProductVersion {Id = 105, ProductId = 100, Version = "2.1-Beta", IsPublic = false},
                    new ProductVersion {Id = 102, ProductId = 101, Version = "3.0", IsPublic = true},
                    new ProductVersion {Id = 103, ProductId = 102, Version = "1.1", IsPublic = true},
                    new ProductVersion {Id = 104, ProductId = 103, Version = "1.0"}
                };
                context.ProductVersions.AddRange(productVersions);
                context.SaveChanges();
            }

            // If there are no Releases and ReleaseNotes
            if (!context.ReleaseNotes.Any() && !context.Releases.Any())
            {
                const string htmlDummy1 =
                    "<div><div>Fixed the shangala bangala<br></div><div><img " +
                    "src=\"https://dev.azure.com/ReleaseNoteSystem/399f705f-cd58-45f2-becb-f890cb50f774/_apis/wit/attachments/9a3d3458-8831-4eb4-af7d-adf0269c8e32?fileName=image.png\" " +
                    "alt=Image></div><div><br></div><div><ol><li>Lars Skriver Mye</li><li>Lars Skriver Mye</li><li>Lars " +
                    "Skriver Mye</li><li>Lars Skriver Mye</li><li>Lars Skriver Mye</li><li>Lars Skriver MyeLars <b>Skriver</b> " +
                    "MyeLars <i>Skriver Mye</i><br></li></ol></div><span>&#128521;</span><span>&#128515;</span></div><blockquote " +
                    "style=\"margin-top:0px;margin-bottom:0px;\"><blockquote style=\"margin-top:0px;margin-bottom:0px;\"><blockquote " +
                    "style=\"margin-top:0px;margin-bottom:0px;\"><div>fafaas</div></blockquote></blockquote><div>Lars Skriver " +
                    "Mye</div></blockquote><div><br></div><div><br></div><div><h1>Lars Skriver Mye<br></h1></div><div><h2></h2></div>";
                const string htmlDummy2 = "<div>fix house-arrest bug</div>";
                const string htmlDummy3 = "<div>Eliminated all escapists</div>";
                const string htmlDummy4 = "<div>Whoa whoa hey hey hey</div>";
                const string htmlDummy5 =
                    "<p>Rudolf Blodstrupmoen er en fiktiv figur som ble skapt av Kjell Aukrust. Aukrust introduserte " +
                    "disponent Blodstrupmoen i Mannskapsavisas avisspalte Våre Duster og overførte ham senere til den " +
                    "fiktive avisen Flåklypa Tidende.</p><p> Disponent Blodstrupmoen var barnefødt i Flåklypa, og jobbet på Reodor Felgens verksted som lærling. Etter hvert flyttet han til byen der han både ble disponent i «Christiania Mark og Soppkontroll» og en dyktig racerbilfører. </p> <p>Rudolf Blodstrupmoen tåler ikke å tape og gjør hva som helst for å vinne. I filmen Flåklypa Grand Prix spilte Blodstrupmoen «skurken» og var førstefører i racerbilen GT Boomerang Rapido (bygget etter «spitfire»-prinsippet). Andrefører var den synske Mysil Bergsprekken, som med sine forutseende evner var en ideell andrefører</p>";
                const string htmlDummy6 =
                    "<p>For å slette faktura kan man nå gå inn i faktura-systemet, velge den fakturaen man ønsker å " +
                    "slette</p><p>Deretter får man opp et nytt vindu som krever bekreftelse på valgt sletting. " +
                    "Dette vil slette fakturaen helt fra systemet.</p>";
                const string htmlDummy7 = "<h3>Flere fordeler over tradisjonell nedskytning</h3>" +
                                          "<p>Det amerikanske forsvarsdepartementet har inngått samarbeid med Fortem " +
                                          "Technologies for å bruke deres AI-baserte Drone Hunter – som i seg " +
                                          "selv er en drone som målrettet går etter andre droner og fanger disse i et nett.</p>";
                const string htmlDummy8 =
                    "<h3>Windows 10X får ny filutforsker, det får kanskje Windows 10 også i fremtiden</h3>" +
                    "<p>For vi ser ingen grunn til at Microsoft ikke prøver å gjøre 10 og 10X så like hverandre som " +
                    "mulig, det er jo meningen man sømløst skal kunne bytte mellom en rekke Windows-enheter.</p>" +
                    "<p>Uansett er dette en testversjon til Windows 10X der de er avduket at man kan bla i lokale " +
                    "diskstasjoner, men at hovedfokuset er OneDrive, noe som gir mening i et moderne operativsystem, " +
                    "spesielt på enheter som alltid er tilkoblet.</p>";
                const string htmlDummy9 = "<h3>Command & Conquer feirer seriens 25 år</h3>" +
                                          "<p>«Samlingen er utviklet i tett samarbeid med Command & Conquer-tilhengere, " +
                                          "og byr på grafikk som har blitt bygget opp fra grunnen av, støtte for " +
                                          "4K-oppløsning, og lydspor som har blitt gjenskap i ytterste kvalitet av den " +
                                          "originale komponisten, Frank Klepacki. Samlingen vil være tilgjengelig den 5. " +
                                          "juni, og kan forhåndsbestilles nå», forklarer EA.</p>" +
                                          "<p>Spillet lanseres på Origin, såvel som Steam.</p>";
                var releaseNotes = new List<ReleaseNote>
                {
                    new ReleaseNote
                    {
                        WorkItemId = 20, AuthorName = "Ronnay Voudrait", AuthorEmail = "ronnay@natron.no",
                        Title = "Sletting av faktura", Ingress = "Det er nå mulig å slette faktura i faktura-systemet",
                        Description = htmlDummy6, WorkItemDescriptionHtml = htmlDummy1,
                        WorkItemTitle = "Test item please ignore",
                        ClosedDate = new DateTime(2019, 11, 05, 10, 52, 02, 22),
                        IsPublic = true
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 21, AuthorName = "Aung San Suu Kyi", AuthorEmail = "aungsansuukyi@natron.no",
                        Title = "Dette er Microsofts nye filutforsker",
                        Ingress =
                            "Microsofts nye filutforsker gir trolig et bilde inn i fremtiden også for Windows 10.",
                        Description = htmlDummy8,
                        WorkItemDescriptionHtml = htmlDummy8, WorkItemTitle = "Receive Nobel Price",
                        ClosedDate = new DateTime(2020, 01, 16, 16, 8, 24, 44),
                        IsPublic = false
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 22, AuthorName = "Liu chi", AuthorEmail = "luichi@natron.no",
                        Title = "Command & Conquer Remastered Collection lanseres 5. juni",
                        Ingress =
                            "Electronic Arts har avslørt lanseringsdatoen for «Command & Conquer Remastered Collection».",
                        Description = htmlDummy9,
                        WorkItemDescriptionHtml = htmlDummy3, WorkItemTitle = "Forbid escapism",
                        ClosedDate = new DateTime(2019, 11, 04, 09, 50, 00, 0),
                        IsPublic = true
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 23, AuthorName = "Lost Sight", AuthorEmail = "IMLSXYZ9@gmail.com",
                        Title = "Pentagon betaler flere millioner for nytt forsvarssystem – skal fange droner i nett",
                        Ingress =
                            "Pentagon har allerede alle tillatelser de trenger for å skyte ned droner de oppfatter som trusler, men nå tar de steget videre for å gjøre dette til en mer effektiv sak.",
                        Description = htmlDummy7,
                        WorkItemDescriptionHtml = htmlDummy2, WorkItemTitle = "Cut the western bullshit",
                        ClosedDate = new DateTime(1989, 3, 16, 02, 22, 42, 1),
                        IsPublic = true
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 24, AuthorName = "Ronnay Voudrait", AuthorEmail = "ronnay@natron.no",
                        Title = "Trump bygger vegg mot Corona", Ingress = "Kina skal betale for veggen",
                        Description = "Det hjelper fint lite, sier forskere",
                        WorkItemDescriptionHtml = htmlDummy4, WorkItemTitle = "Fix issues with the application",
                        ClosedDate = new DateTime(2005, 7, 11, 14, 0, 59, 4),
                        IsPublic = true
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 25, AuthorName = "Ronnay Voudrait", AuthorEmail = "ronnay@natron.no",
                        Title = "Rudolf Blodstrupmoen", Ingress = "Hvem er denne mannen?",
                        Description = htmlDummy5,
                        WorkItemDescriptionHtml = htmlDummy5, WorkItemTitle = "Fix issues with the application",
                        ClosedDate = new DateTime(2005, 7, 11, 14, 0, 59, 4),
                        IsPublic = true
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 26, AuthorName = "Ronnay Voudrait", AuthorEmail = "ronnay@natron.no",
                        Title = "", Ingress = "",
                        Description = "Kan nå bruke websiden på Internet Explorer 6, selv om dette IKKE er anbefalt",
                        WorkItemDescriptionHtml = htmlDummy4, WorkItemTitle = "Fix issues with the application",
                        ClosedDate = new DateTime(2005, 7, 11, 14, 0, 59, 4),
                        IsPublic = true
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 27, AuthorName = "Ronnay Voudrait", AuthorEmail = "ronnay@natron.no",
                        Title = "", Ingress = "",
                        Description = "Nettleseren koker ikke lengre over når man legger inn faktura.",
                        WorkItemDescriptionHtml = htmlDummy4, WorkItemTitle = "Fix issues with the application",
                        ClosedDate = new DateTime(2005, 7, 11, 14, 0, 59, 4),
                        IsPublic = true
                    },
                    new ReleaseNote
                    {
                        WorkItemId = 27, AuthorName = "Ronnay Voudrait", AuthorEmail = "ronnay@natron.no",
                        Title = "", Ingress = "",
                        Description = "Nettsiden har nå støtte for autofyll.",
                        WorkItemDescriptionHtml = htmlDummy4, WorkItemTitle = "Fix issues with the application",
                        ClosedDate = new DateTime(2005, 7, 11, 14, 0, 59, 4),
                        IsPublic = false
                    }
                };

                var releases = new List<Release>
                {
                    new Release {Id = 100, ProductVersionId = 100, Title = "Release 2.1", IsPublic = true},
                    new Release
                    {
                        Id = 101, ProductVersionId = 101, Title = "Release 2.2", IsPublic = false,
                        Date = new DateTime(2012, 7, 13, 16, 8, 24, 44)
                    },
                    new Release
                    {
                        Id = 102, ProductVersionId = 102, Title = "Release 2.3", IsPublic = true,
                        Date = new DateTime(2013, 1, 26, 16, 8, 24, 44)
                    },
                    new Release
                    {
                        Id = 103, ProductVersionId = 103, Title = "Release 3.1", IsPublic = false,
                        Date = new DateTime(2014, 2, 11, 16, 8, 24, 44)
                    },
                    new Release
                    {
                        Id = 104, ProductVersionId = 103,
                        Title = "Release 3.2", IsPublic = true,
                        Date = new DateTime(2015, 3, 12, 16, 8, 24, 44)
                    },
                    new Release
                    {
                        Id = 105, ProductVersionId = 102, Title = "Release 3.3", IsPublic = false,
                        Date = new DateTime(2016, 5, 25, 16, 8, 24, 44)
                    }
                };
                var releaseReleaseNotes = new List<ReleaseReleaseNote>();

                foreach (var releaseNote in releaseNotes)
                {
                    releaseReleaseNotes.Add(new ReleaseReleaseNote
                        {ReleaseNote = releaseNote, Release = releases[0]});
                }

                context.Releases.AddRange(releases);
                context.ReleaseNotes.AddRange(releaseNotes);
                context.ReleaseReleaseNotes.AddRange(releaseReleaseNotes);
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
                    new User
                    {
                        Email = "admin@ungspiller.no", Password = passwordHasher.HashPassword("12345678"),
                        AzureInformation = new AzureInformation
                        {
                            Id = 123, UserId = "test.testovitch@testnes.no", Pat = "123123asdasd",
                            Organization = "ReleaseNoteSystem"
                        }
                    },
                    new User
                    {
                        Email = "common@ungspiller.no", Password = passwordHasher.HashPassword("12345678")
                    }
                };

                users[0].UserRoles.Add(new UserRole
                {
                    RoleId = context.Roles.SingleOrDefault(
                        r => r.Name == ERole.Administrator.ToString()).Id
                });

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.MappableTypes.Any())
            {
                var types = new List<MappableType>
                {
                    new MappableType {Name = "task"},
                    new MappableType {Name = "bug"},
                };
                context.MappableTypes.AddRange(types);
                context.SaveChanges();
            }

            // If there are no mappings
            if (!context.MappableFields.Any())
            {
                var fields = new List<MappableField>
                {
                    new MappableField {Name = "Title", DataType = MappableDataTypes.String},
                    new MappableField {Name = "Ingress", DataType = MappableDataTypes.String},
                    new MappableField {Name = "Description", DataType = MappableDataTypes.Html},
                    new MappableField {Name = "WorkItemDescriptionHtml", DataType = MappableDataTypes.Html},
                    new MappableField {Name = "WorkItemTitle", DataType = MappableDataTypes.String},
                    new MappableField {Name = "ClosedDate", DataType = MappableDataTypes.DateTime}
                };

                var defaultMappings = new Dictionary<string, string>
                {
                    {fields[0].Name, "System.Title"},
                    {fields[1].Name, null},
                    {fields[2].Name, "System.Description"},
                    {fields[3].Name, "System.Description"},
                    {fields[4].Name, "System.Title"},
                    {fields[5].Name, "Microsoft.VSTS.Common.StateChangeDate"},
                };
                
                context.MappableFields.AddRange(fields);
                if (!context.ReleaseNoteMappings.Any())
                {
                    var mappings = new List<ReleaseNoteMapping>();
                    foreach (var mappableType in context.MappableTypes.ToList())
                    {
                        foreach (var mappableField in fields)
                        {
                            defaultMappings.TryGetValue(mappableField.Name, out var defaultMap);
                            mappings.Add(new ReleaseNoteMapping
                                {MappableField = mappableField, MappableType = mappableType, AzureDevOpsField = defaultMap});
                        }
                    }

                    context.AddRange(mappings);
                }

                context.SaveChanges();
            }
        }
    }
}