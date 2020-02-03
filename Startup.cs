using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ReleaseNotes_WebAPI.API.Services;
using ReleaseNotes_WebAPI.Domain.Models.Auth.Token;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Security;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Persistence.Contexts;
using ReleaseNotes_WebAPI.Persistence.Repositories;
using ReleaseNotes_WebAPI.Security.Tokens;
using ReleaseNotes_WebAPI.Services;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ADDS DATABASE SERVICE
            // CONNECTS WEB-API TO DB
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<IPasswordHasher, Security.Hashing.PasswordHasher>();
            services.AddSingleton<ITokenHandler, TokenHandler>();

            // BIND ALL REPOS
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();

            // BIND ALL SERVICES
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // CONFIGURE AUTHENTICATION AND TOKEN OPTIONS
            services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));
            var tokenOptions = Configuration.GetSection("TokenOptions")
                .Get<TokenOptions>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
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
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // SHOULD BE ENABLED IN PRODUCTION ENVIRONMENTS
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}