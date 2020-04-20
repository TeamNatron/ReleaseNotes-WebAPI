using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ReleaseNotes_WebAPI.API.Services;
using ReleaseNotes_WebAPI.Domain.Models.Auth.Token;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Security;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.ModelBinder;
using ReleaseNotes_WebAPI.Persistence.Contexts;
using ReleaseNotes_WebAPI.Persistence.Repositories;
using ReleaseNotes_WebAPI.Security.Tokens;
using ReleaseNotes_WebApi.Services;
using ReleaseNotes_WebAPI.Services;
using ReleaseNotesWebAPI.Services;
using TokenHandler = ReleaseNotes_WebAPI.Security.Tokens.TokenHandler;

namespace ReleaseNotes_WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // GRABBING ENVIRONMENT VARIABLES
            // Need to do the construction of the connectionString inside StartUp,
            // since it's hard to do string manipulation in docker-world
            var host = Environment.GetEnvironmentVariable("HOST");
            var port = Environment.GetEnvironmentVariable("PORT");
            var db = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var passw = Environment.GetEnvironmentVariable("DB_PASSW");
            var connectionString =
                "host=" + host + ";port=" + port + ";database=" + db + ";username=" + user + ";password=" + passw + ";";

            services.AddHttpContextAccessor();
            // ADDS DATABASE SERVICE
            // CONNECTS WEB-API TO DB
            services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); });

            // Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
            
            // Give developers access to HttpContext
            services.AddHttpContextAccessor(); 

            services.AddSingleton<IPasswordHasher, Security.Hashing.PasswordHasher>();
            services.AddSingleton<ITokenHandler, TokenHandler>();

            // BIND ALL REPOS
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IReleaseNoteRepository, ReleaseNoteRepository>();
            services.AddScoped<IReleaseRepository, ReleaseRepository>();
            services.AddScoped<IProductVersionRepository, ProductVersionRepository>();
            services.AddScoped<IAzureInformationRepository, AzureInformationRepository>();
            services.AddScoped<IMappableRepository, MappableRepository>();

            // BIND ALL SERVICES
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReleaseNoteService, ReleaseNoteService>();
            services.AddScoped<IReleaseService, ReleaseService>();
            services.AddScoped<IProductVersionService, ProductVersionService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IMappableService, MappableService>();

            // CONFIGURE AUTHENTICATION AND TOKEN OPTIONS
            services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));
            var tokenOptions = Configuration.GetSection("TokenOptions")
                .Get<TokenOptions>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        IssuerSigningKey = signingConfigurations.Key,
                        ClockSkew = TimeSpan.Zero
                    };
                    // jwtBearerOptions.Events = new JwtBearerEvents()
                    // {
                    //     OnChallenge = context =>
                    //     {
                    //         Console.WriteLine("OnChallenge: " + context.Response.StatusCode);
                    //         return Task.CompletedTask;
                    //     },
                    //     OnAuthenticationFailed = context =>
                    //     {
                    //         Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                    //         return Task.CompletedTask;
                    //     },
                    //     OnForbidden = context =>
                    //     {
                    //         Console.WriteLine("OnForbidden: " + context.Response.StatusCode);
                    //         return Task.CompletedTask;
                    //     },
                    //     OnMessageReceived = context =>
                    //     {
                    //         Console.WriteLine("OnMessageReceived: " + context.Response.StatusCode);
                    //         return Task.CompletedTask;
                    //     },
                    //     OnTokenValidated = context =>
                    //     {
                    //         Console.WriteLine("OnTokenValidated: " + context.Response.StatusCode);
                    //         return Task.CompletedTask;
                    //     }
                    // };
                });

            // May be wrong to use current parameter, was added to resolve error
            services.AddAutoMapper(typeof(Startup));

            // ADD ALL CONTROLLERS (ENDPOINTS)
            services.AddControllers(
                options => { options.ModelBinderProviders.Insert(0, new BooleanModelBinderProvider()); }
            ).AddNewtonsoftJson(
                options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            // Init static file serving
            var webRoot = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            env.WebRootPath = webRoot.FullName;
            Directory.CreateDirectory(Path.Combine(env.WebRootPath, "images"));
            app.UseStaticFiles();
            
            // SHOULD BE ENABLED IN PRODUCTION ENVIRONMENTS
            // app.UseHttpsRedirections();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}