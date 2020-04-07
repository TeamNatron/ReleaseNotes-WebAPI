using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Models.Auth.Token;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Resources.Auth;

namespace ReleaseNotes_WebAPI.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<ProductVersion, CreateProductVersionResource>();
            CreateMap<User, UserResource>()
                .ForMember(u => u.Roles, opt => opt.MapFrom(u => u.UserRoles.Select(ur => ur.Role.Name)));
            CreateMap<User, UserDetailedResource>()
                .ForMember(u => u.Roles, opt => opt.MapFrom(u => u.UserRoles.Select(ur => ur.Role.Name)));

            CreateMap<AccessToken, AccessTokenResource>()
                .ForMember(
                    a => a.AccessToken,
                    opt => opt.MapFrom(
                        a => a.Token))
                .ForMember(a => a.RefreshToken,
                    opt => opt.MapFrom(
                        a => a.RefreshToken.Token))
                .ForMember(a => a.Expiration,
                    opt => opt.MapFrom(
                        a => a.Expiration));

            CreateMap<Article, ArticleResource>();
            // CreateMap<Release, ReleaseResource>();
            CreateMap<Release, ReleaseResource>()
                .ForMember(rr => rr.ReleaseNotes,
                    opt => opt.MapFrom(
                        r => r.ReleaseReleaseNotes.Select(rrn => rrn.ReleaseNote)));

            CreateMap<ProductVersion, ProductVersionResource>()
                .ForMember(pvr => pvr.FullName,
                    opt =>
                        opt.MapFrom(pv => pv.Product.Name + " " + pv.Version));

            CreateMap<Product, ProductResource>()
                .ForMember(dest => dest.Versions, 
                    opt => 
                        opt.MapFrom(src => src.ProductVersions));

            CreateMap<ReleaseNote, ReleaseNoteResource>()
                .ForMember(dest => dest.Releases,
                    opt =>
                        opt.MapFrom(src =>
                            src.ReleaseReleaseNotes.Select(rrn => rrn.Release).Select(r => 
                                new Release() {Date = r.Date, Id = r.Id,Title = r.Title,IsPublic = r.IsPublic,ProductVersion = r.ProductVersion})));
        }
    }
}